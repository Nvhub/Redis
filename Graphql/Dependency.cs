using Redis.Graphql.Queries;
using Redis.Graphql.Mutations;


namespace Redis.Graphql
{
    public static class Dependency
    {
        public static IServiceCollection AddGraphqlDependency(this IServiceCollection services)
        {
            services.AddScoped<ArticleQuery>();
            services.AddScoped<ArticleMutation>();
            return services;
        }
    }
}
