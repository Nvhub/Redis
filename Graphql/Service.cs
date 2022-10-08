using Redis.Graphql.Queries;
using Redis.Graphql.Utils;
using Redis.Graphql.Mutations;


namespace Redis.Graphql
{
    public static class Service
    {
        public static IServiceCollection AddGraphqlService(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddErrorFilter<ErrorHandler>()
                .AddMutationType<ArticleMutation>()
                .AddQueryType<ArticleQuery>();

            return services;
        }
    }
}
