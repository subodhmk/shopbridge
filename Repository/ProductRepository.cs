using Microsoft.Extensions.Logging;
using Shopbridge_base.Data;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Shopbridge_Context _db;
        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(Shopbridge_Context db,ILogger<ProductRepository> _logger)
        {
            _db = db;
            logger = _logger;
        }   

        public bool CreateProduct(Product product)
        {
            try
            {
                _db.Product.Add(product);
                return Save();
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return false;
            }
        }

        public bool DeleteProduct(Product product)
        {
            try
            {
                _db.Product.Remove(product);
                return Save();
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return false;
            }
        }

        public Product GetProduct(int id)
        {
            try
            {
                return _db.Product.FirstOrDefault(a => a.Product_Id == id);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return null;
            }
        }

        public ICollection<Product> GetProducts()
        {
            try
            {
                return _db.Product.OrderBy(a => a.Name).ToList();
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return null;
            }
        }

        public bool ProductExists(string productName)
        {
            try
            {
                return _db.Product.Any(a => a.Name.ToLower().Trim() == productName.ToLower().Trim());
            }
            catch (Exception Ex)
            {
                logger.LogTrace(Ex.Message);
                return false;
            }
        }

        public bool ProductExists(int productId)
        {
            try
            {
                return _db.Product.Any(a => a.Product_Id == productId);
            }
            catch (Exception Ex)
            {
                logger.LogTrace(Ex.Message);
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                return _db.SaveChanges() >= 0 ? true : false;
            }
            catch (Exception Ex)
            {
                logger.LogTrace(Ex.Message);
                return false;
            }
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                _db.Product.Update(product);
                return Save();
            }
            catch (Exception Ex)
            {
                logger.LogTrace(Ex.Message);
                return false;
            }
        }
    }
}
