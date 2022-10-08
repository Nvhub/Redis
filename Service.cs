using Redis.Graphql;
using Redis.Redis;
using Redis.Models;

namespace Redis
{
    public static class Service
    {
        public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddModelService(builder);
            services.AddRedisService();
            services.AddGraphqlService();
            return services;
        }
    }
}
