using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop_db.Models
{
    [PrimaryKey(nameof(Id))]
    public class ImageData
    {
        public Guid Id { get; set; }
        [MaxLength(1_000_000)]
        public byte[] Data { get; set; }
        public long Length { get; set; }
    }
}
