using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_api.Model;
using shop_db;
using shop_db.Models;

namespace shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Items : ControllerBase
    {
        ShopContext DB { get; set; }

        public Items(ShopContext db)
        {
            DB = db;
        }

        [HttpGet("/all")]
        public async Task<List<Product>> GetAllItems()
        {
            return await DB.Products.ToListAsync();
        }

        [HttpGet("item/{id}")]
        public async Task<Product> GetItem(Guid id)
        {
            return await DB.Products.FirstOrDefaultAsync(i => i.Id == id) ?? throw new ArgumentException("Item does not exist.");
        }

        [HttpPost("/add")]
        public async Task<IActionResult> AddItem([FromBody] Product item)
        {
            DB.Products.Add(item);
            await DB.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("/item/{id}/images")]
        public async Task<List<ProductImageQueryResult>> ProductImages(Guid id)
        {
            var images = await DB.Products
                .Include(p => p.ProductImages)
                .Where(i => i.Id == id)
                .SelectMany(i => i.ProductImages)
                .Select(i => new { 
                    Description = i.Description,
                    ImageId = (Guid?)i.Image.Id,
                    ImageLen = (long?)i.Image.Length,
                    ThumbnailId = (Guid?)i.Thumbnail.Id,
                    ThumbnailLen = (long?)i.Thumbnail.Length
                }).ToListAsync();

            var list = images.Select(i => new ProductImageQueryResult()
            {
                ImageUrl = $"/image/{i.ImageId}",
                ImageLength = i.ImageLen ?? 0,
                ThumbnailUrl = $"/image/{i.ThumbnailId}",
                ThumbnailLength = i.ThumbnailLen ?? 0,
                Description = i.Description
            }).ToList();

            return list;
        }

        [HttpGet("/image/{id}")]
        public async Task<ImageData> GetImage(Guid id)
        {
            var image = await DB.Images.FirstOrDefaultAsync(i => i.Id == id);
            return image ?? throw new Exception("Image is not available");
        }

        [HttpPatch("/update/{id}")]
        public async Task<Product> UpdateItem(Guid id, [FromBody] Product item)
        {
            var current = await DB.Products.FirstOrDefaultAsync(i => i.Id == id);
            if (current == null)
            {
                throw new ArgumentException("Item does not exist.");
            }
            current.Name = item.Name;
            current.Description = item.Description;
            current.Inventory = item.Inventory;
            current.Price = item.Price;
            DB.Products.Update(current);
            await DB.SaveChangesAsync();
            return current;
        }
    }
}
