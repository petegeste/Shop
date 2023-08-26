using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_db;

namespace shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        ShopContext DB { get; set; }

        public Users(ShopContext db)
        {
            DB = db;
        }

        [HttpGet("/{user}")]
        public async Task<IActionResult> GetUser([FromQuery] Guid user)
        {
            return Ok(await DB.Users.FirstOrDefaultAsync(u => u.Id == user));
        }
    }
}
