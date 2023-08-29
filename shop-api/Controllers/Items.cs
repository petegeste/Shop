using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        [HttpGet("/item/{id}")]
        public async Task<Product> GetItem(Guid id)
        {
            return await DB.Products.FirstOrDefaultAsync(i => i.Id == id) ?? throw new ArgumentException("Item does not exist.");
        }

        [HttpPost("/add")]
        public async Task<IActionResult> AddItem([FromBody] Product item)
        {
            var imgs = item.ProductImages.Select(i => i.Id).ToList();
            item.ProductImages.Clear();
            foreach (var guid in imgs)
            {
                var img = await DB.ProductImages.FirstOrDefaultAsync(i => i.Id == guid);
                if (img is null)
                {
                    continue;
                }

                item.ProductImages.Add(img);
            }
            DB.Products.Add(item);
            await DB.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("/item/{id}")]
        public async Task DeleteItem(Guid id)
        {
            var product = await DB.Products
                .Include(p => p.ProductImages)
                .ThenInclude(i => i.Image)
                .Include(p => p.ProductImages)
                .ThenInclude(i => i.Thumbnail)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (product is null)
            {
                throw new Exception("The product does not exist.");
            }

            var thumbnails = product.ProductImages.Select(i => i.Thumbnail).Where(t => t is not null).Cast<ImageData>();
            var images = product.ProductImages.Select(i => i.Image).Where(t => t is not null).Cast<ImageData>();
            DB.RemoveRange(images);
            DB.RemoveRange(thumbnails);
            DB.RemoveRange(product.ProductImages);
            DB.Remove(product);
            await DB.SaveChangesAsync();
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
                ImageUrl = $"/api/image/{i.ImageId}",
                ImageLength = i.ImageLen ?? 0,
                ThumbnailUrl = $"/api/image/{i.ThumbnailId}",
                ThumbnailLength = i.ThumbnailLen ?? 0,
                Description = i.Description
            }).ToList();

            return list;
        }

        [HttpGet("/image/{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await DB.Images.FirstOrDefaultAsync(i => i.Id == id);

            if (image is null)
            {
                throw new Exception("Image is not available");
            }

            return File(image.Data, "image/jpeg");
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
