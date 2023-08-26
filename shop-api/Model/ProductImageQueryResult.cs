using shop_db.Models;

namespace shop_api.Model
{
    public class ProductImageQueryResult
    {
        public string ImageUrl { get; set; }
        public long ImageLength { get; set; }
        public string ThumbnailUrl { get; set; }
        public long ThumbnailLength { get; set; }
        public string Description { get; set; }
    }
}
