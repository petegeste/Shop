using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Model;
using shop_db;

namespace shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Images : ControllerBase
    {
        private ShopContext Context { get; set; }

        public Images(ShopContext context)
        {
            Context = context;
        }

        [HttpPost("/images/upload")]
        public async Task<Guid> UploadImage([FromForm] ImageUpload upload)
        {
            var guid = Guid.NewGuid();
            var ext = Path.GetExtension(upload.File.FileName);
            var new_name = $"{guid}{ext}";
            var path = Path.Combine(Environment.GetEnvironmentVariable("SHOP_IMAGE_CACHE")!, new_name);

            try
            {
                using var file = System.IO.File.Create(path);
                await upload.File.CopyToAsync(file);
            }
            catch
            {
                System.IO.File.Delete(path);
                throw new Exception("Upload failed.");
            }

            Context.ProductImages.Add(new shop_db.Models.ProductImage()
            {
                Description = upload.Description,
                Id = guid
            });
            await Context.SaveChangesAsync();

            using var mq = new MQ();
            await mq.RequestImageConvert(new_name);

            return guid;
        }
    }
}
