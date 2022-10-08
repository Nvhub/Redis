using Microsoft.EntityFrameworkCore;

namespace Redis.Models
{
    public static class Service
    {
        public static IServiceCollection AddModelService(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
            return services;
        }
    }
}
