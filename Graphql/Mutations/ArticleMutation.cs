using Redis.Models;
using Redis.Redis;
using Redis.Services.Repository.Impelement;

namespace Redis.Graphql.Mutations
{
    public class ArticleMutation
    {
        const string cacheKey = "articleKey";
        DateTime expire = DateTime.Now.AddSeconds(25);

        public async Task<List<Article>> AddUser([Service] CacheArticle cacheArticle,
            [Service] IArticleRepository articleRepository, string title)
        {
            Article form = new Article()
            {
                Title = title,
            };
            Article article = await articleRepository.AddArticleAsync(form);
            List<Article> articles = await cacheArticle.cacheAdd(cacheKey, article, expire);
            return articles;
        }

        public async Task<Article> removeUser([Service] CacheArticle cacheArticle,
            [Service] IArticleRepository articleRepository, int id)
        {
            Article article = await cacheArticle.cacheDelete(cacheKey, id, expire);
            if (article is null)
                throw new Exception($"article with id {id} not found");
            await articleRepository.DeleteArticleByIdAsync(article);
            return article;
        }

        public async Task<Article> updateUser([Service] CacheArticle cacheArticle,
            [Service] IArticleRepository articleRepository, int id, string title)
        {
            Article form = new Article();
            form.Title = title;
            Article article = await cacheArticle.cacheUpdate(cacheKey, id, form, expire);
            if(article is null)
                throw new Exception($"article with id {id} not found");
            article = await articleRepository.UpdateArticleByIdAsync(article);
            return article;
        }
    }
}
