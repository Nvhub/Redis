namespace Redis.Redis
{
    public static class Dependency
    {
        public static IServiceCollection AddRedisDependency(this IServiceCollection services)
        {
            services.AddScoped<CacheArticle>();
            return services;
        }
    }
}
