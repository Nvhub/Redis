using Microsoft.AspNetCore.Mvc;
using Redis.Models;
using Redis.Redis;
using Redis.Services.Repository.Impelement;

namespace Redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly CacheArticle _cacheArticle;
        private readonly IArticleRepository _articleRepository;
        const string cacheKeyArticle = "ArticleList";
        DateTime expire = DateTime.Now.AddSeconds(30);

        public WeatherForecastController(CacheArticle cacheArticle, IArticleRepository articleRepository)
        {
            _cacheArticle = cacheArticle;
            _articleRepository = articleRepository;
        }

        // CRUD with Redis Cache Server

        // ROW ALL
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Article> articles = await _cacheArticle.cacheList(cacheKeyArticle, expire);
            return Ok(articles);
        }   
        
        // ROW
        [Route("/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            List<Article> articles = await _cacheArticle.cacheList(cacheKeyArticle, expire);
            Article article = articles.FirstOrDefault(user => user.Id == id);
            if (article is null)
                return NotFound($"article with id {id} not found");
            return Ok(article);
        }

        // CREATE
        [Route("/")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Article body)
        {
            var article = await _articleRepository.AddArticleAsync(body);
            List<Article> articles = await _cacheArticle.cacheAdd(cacheKeyArticle, article, expire);
            return Ok(articles);
        }

        // DELETE
        [Route("/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _cacheArticle.cacheDelete(cacheKeyArticle, id, expire);
            if (article is null)
                return NotFound($"article with id {id} not found");
            await _articleRepository.DeleteArticleByIdAsync(article);
            return Ok(article);
        }
    
        // TODO : UPDATE
        [Route("/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, Article body)
        {
            var article = await _cacheArticle.cacheUpdate(cacheKeyArticle,id, body, expire);
            if (article is null)
                return NotFound($"article with id {id} not found");
            await _articleRepository.UpdateArticleByIdAsync(article);
            return Ok(article);
        }
    }
}