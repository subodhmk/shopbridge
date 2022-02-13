using Shopbridge_base.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Repository.IRepository
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int id);
        bool ProductExists(string productName);
        bool ProductExists(int productId);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
    }
}
