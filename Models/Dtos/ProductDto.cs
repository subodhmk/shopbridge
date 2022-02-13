using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Domain.Models.Dtos
{
    public class ProductDto
    {
        private const string V = @"/^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$/";

        public int Product_Id { get; set; }

        [Required(ErrorMessage = "Product Name Required")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only Alphabets allowed.")]
        [MinLength(3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product Price Required")]
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        [RegularExpression(V, ErrorMessage = "Only Numbers allowed.")]
        public Decimal Price { get; set; }

        [Required(ErrorMessage = "Product Description Required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [MinLength(4)]
        public string Discrption { get; set; }
    }
}
