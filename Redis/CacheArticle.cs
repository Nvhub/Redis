using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.Models;
using Redis.Services.Repository.Impelement;
using System.Text;

namespace Redis.Redis
{
    public class CacheArticle
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IArticleRepository _articleRepository;
        private readonly Context _context;

        public CacheArticle(IDistributedCache distributedCache,  IArticleRepository articleRepository, Context context)
        {
            _distributedCache = distributedCache;
            _articleRepository = articleRepository;
            _context = context;
        }

        private List<T> deserializeObject<T>(byte[] redisList)
        {
            var serializedList = Encoding.UTF8.GetString(redisList);
            return JsonConvert.DeserializeObject<List<T>>(serializedList);
        }

        private byte[] serializeObject<T>(List<T> data)
        {
            var serializedList = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(serializedList);
        }

        private DistributedCacheEntryOptions entryOptions(DateTime expire)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = expire;
            return options;
        }

        public async Task<List<Article>> cacheList(string cacheKey, DateTime expire)
        {
            var redisList = await _distributedCache.GetAsync(cacheKey);
            List<Article> articles = new List<Article>();
            if (redisList is not null)
            {
                articles = deserializeObject<Article>(redisList);
            }
            else
            {
                articles = await _articleRepository.GetAllArticlesAsync();
                redisList = serializeObject(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
            }
            return articles;
        }

      
        public async Task<List<Article>> cacheAdd(string cacheKey, Article article, DateTime expire)
        {
            var redisList = await _distributedCache.GetAsync(cacheKey);
            List<Article> articles = new List<Article>();
            if (redisList is not null)
            {
                articles = deserializeObject<Article>(redisList);
                articles.Add(article);
                redisList = serializeObject<Article>(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
            }
            else
            {
                articles = await _articleRepository.GetAllArticlesAsync();
                redisList = serializeObject<Article>(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
            }
            return articles;
        }
        public async Task<Article> cacheDelete(string cacheKey, int id, DateTime expire)
        {
            List<Article> articles = new List<Article>();
            Article article = new Article();
            var redisList = await _distributedCache.GetAsync(cacheKey);
            if (redisList is not null)
            {
                articles = deserializeObject<Article>(redisList);
                article = articles.FirstOrDefault(article => article.Id == id);
                if (article is null)
                    return null;
                articles = articles.FindAll(article => article.Id != id).ToList();
                redisList = serializeObject<Article>(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
            }
            else
            {
                articles = await _articleRepository.GetAllArticlesAsync();
                article = articles.FirstOrDefault(article => article.Id == id);
                articles = articles.FindAll(article => article.Id != id);
                redisList = serializeObject<Article>(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
                if (article is null)
                    return null;
            }
            return article;

        }
        public async Task<Article> cacheUpdate(string cacheKey, int id,Article body, DateTime expire)
        {
            List<Article> articles = new List<Article>();
            Article article = new Article();
            var redisList = await _distributedCache.GetAsync(cacheKey);
            if(redisList is not null)
            {
                articles = deserializeObject<Article>(redisList);
                article = articles.SingleOrDefault(article => article.Id == id);
                if (article is null)
                    return null;
                article.Title = body.Title;
                redisList = serializeObject(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
            }
            else
            {
                articles = await _articleRepository.GetAllArticlesAsync();
                article = articles.FirstOrDefault(article => article.Id == id);
                if (article == null)
                    return null;
                article.Title = body.Title;
                redisList = serializeObject(articles);
                await _distributedCache.SetAsync(cacheKey, redisList, entryOptions(expire));
            }
            return article;
        }
    }
}
