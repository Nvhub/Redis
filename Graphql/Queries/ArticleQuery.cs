using Redis.Models;
using Redis.Redis;

namespace Redis.Graphql.Queries
{
    public class ArticleQuery
    {
        const string cacheKey = "articleKey";
        DateTime expire = DateTime.Now.AddSeconds(25);
        public async Task<List<Article>> GetAllArticles([Service] CacheArticle cacheArticle)
        {
            List<Article> articles = await cacheArticle.cacheList(cacheKey, expire);
            return articles;
        }

        public async Task<Article> GetArticle([Service] CacheArticle cacheArticle, int id)
        {
            List<Article> articles = await cacheArticle.cacheList(cacheKey, expire);
            Article article = articles.SingleOrDefault(article => article.Id == id);
            if (article is null)
                throw new Exception($"article with id {id} not found");
            return article;
        }
    }
}
