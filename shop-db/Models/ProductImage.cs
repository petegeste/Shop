using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace shop_db.Models
{
    [PrimaryKey(nameof(Id))]
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ImageData? Image { get; set; }
        public ImageData? Thumbnail { get; set; }
    }
}
