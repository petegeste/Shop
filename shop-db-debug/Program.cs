using shop_db;
using shop_db.Models;

namespace shop_db_debug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var db = new ShopContext();
            var item = new Item()
            {
                Name = "Test",
                Description = "Test test",
                Price = 3.14m,
                Inventory = 100
            };
            db.Items.Add(item);
            db.SaveChanges();
        }
    }
}