using Redis.Services.Repository.Impelement;
using Redis.Services.Repository.Interface;

namespace Redis.Models
{
    public static class Dependency
    {
        public static IServiceCollection AddModelDependency(this IServiceCollection services)
        {
            services.AddTransient<IArticleRepository, ArticleRepository>();
            return services;
        }
    }
}
