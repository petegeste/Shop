using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Item>> All()
        {
            return await DB.Items.ToListAsync();
        }

        [HttpPost("/add")]
        public async Task<IActionResult> PostItem([FromBody] Item item)
        {
            DB.Items.Add(item);
            await DB.SaveChangesAsync();
            return Ok();
        }
    }
}
