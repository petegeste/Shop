using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_db;

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

        [HttpGet]
        public IActionResult All()
        {
            return Ok(DB.Items.ToList());
        }
    }
}
