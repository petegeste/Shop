using Microsoft.EntityFrameworkCore;

namespace shop_db.Models
{
    [PrimaryKey(nameof(Id))]
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }
        public List<ProductImage> ProductImages { get; set; } = null!;
    }
}
