using shop_db;
using shop_db.Models;

namespace shop_db_debug
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var cli = new HttpClient();
            var api = new shop.ApiClient("http://localhost:80", cli);
            var item = new shop.Product()
            {
                Name = "Test",
                Description = "Test desc",
                Price = 1.23,
            };
            await api.AddAsync(item);
        }
    }
}