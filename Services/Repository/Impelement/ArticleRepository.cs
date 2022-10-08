using Microsoft.EntityFrameworkCore;
using Redis.Models;
using Redis.Services.Repository.Impelement;

namespace Redis.Services.Repository.Interface
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly Context _context;

        public ArticleRepository(Context context)
        {
            _context = context;
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(x => x.Id == id);
            return article;
        }

        public async Task<Article> AddArticleAsync(Article article)
        {
            var articleAdd = await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            return articleAdd.Entity;
        }

        public async Task<Article> DeleteArticleByIdAsync(Article article)
        {
            var articleRemove = _context.Articles.Remove(article).Entity;
            await _context.SaveChangesAsync();
            return articleRemove;
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            var articles = await _context.Articles.ToListAsync();
            return articles;
        }

        public async Task<Article> UpdateArticleByIdAsync(Article article)
        {
            var articleEdit = _context.Articles.Update(article).Entity;
            await _context.SaveChangesAsync();
            return articleEdit;
        }
    }
}
