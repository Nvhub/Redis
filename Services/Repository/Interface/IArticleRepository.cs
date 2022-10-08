using Redis.Models;

namespace Redis.Services.Repository.Impelement
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
        Task<Article> DeleteArticleByIdAsync(Article article);
        Task<Article> AddArticleAsync(Article article);
        Task<Article> UpdateArticleByIdAsync(Article article);
    }
}
