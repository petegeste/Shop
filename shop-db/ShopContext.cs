using Microsoft.EntityFrameworkCore;
using shop_db.Models;

namespace shop_db
{
    public class ShopContext : DbContext
    {
        public const string ConnectionStringEnvVar = @"SHOP_DB_CS";

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvVar);
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
