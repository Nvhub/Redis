using Redis.Graphql;
using Redis.Models;
using Redis.Redis;

namespace Redis
{
    public static class Dependency
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddModelDependency();
            services.AddRedisDependency();
            services.AddGraphqlDependency();
            return services;
        }
    }
}
