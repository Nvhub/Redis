using Microsoft.EntityFrameworkCore;

namespace Redis.Models
{
    public class Context : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

    }
}
