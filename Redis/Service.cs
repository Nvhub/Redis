namespace Redis.Redis
{
    public static class Service
    {
        public static IServiceCollection AddRedisService(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";

            });
            return services;
        }
    }
}
