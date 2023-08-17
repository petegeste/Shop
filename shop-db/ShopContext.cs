using Microsoft.EntityFrameworkCore;
using shop_db.Models;

namespace shop_db
{
    public class ShopContext : DbContext
    {
        public const string HostEnvVar = @"SHOP_DB_HOST";
        public const string UserEnvVar = @"SHOP_DB_USER";
        public const string PasswordEnvVar = @"SHOP_DB_PWD";

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BuildConnectionString());
        }

        private static string BuildConnectionString() => $"Host={Environment.GetEnvironmentVariable(HostEnvVar)};" +
            "Database=shop-db;" +
            $"Username={Environment.GetEnvironmentVariable(UserEnvVar)};" +
            $"Password={Environment.GetEnvironmentVariable(PasswordEnvVar)}";
    }
}
