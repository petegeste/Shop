using Microsoft.EntityFrameworkCore;

namespace shop_db.Models
{
    [PrimaryKey(nameof(Id))]
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
