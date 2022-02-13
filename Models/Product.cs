using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Domain.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public Decimal Price { get; set; }
        public string Discrption { get; set; }
    }
}
