using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopbridge_base.Data;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Domain.Models.Dtos;
using Shopbridge_base.Repository.IRepository;

namespace Shopbridge_base.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region INTIT
        private readonly IProductRepository productRepository;
        private readonly ILogger<ProductsController> logger;
        private readonly IMapper mapper;

        public ProductsController(IProductRepository _productRepository, IMapper _mapper,ILogger<ProductsController> _logger)
        {
            this.productRepository = _productRepository;
            this.mapper = _mapper;
            this.logger = _logger;
        } 
        #endregion

        #region GET PRODUCT DETAILS
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProductDto>))]  //For Showing in  swagger documentation  response description
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            try
            {
                var objList = productRepository.GetProducts();
                var objDto = new List<ProductDto>();

                foreach (var item in objList)
                {
                    objDto.Add(mapper.Map<ProductDto>(item));
                }

                return Ok(objDto);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                ModelState.AddModelError("", $"SomeThing Went Wrong");
                return StatusCode(500, ModelState);
            }
        }


        [HttpGet("{productid:int}", Name = "GetProduct")]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize]
        public async Task<ActionResult<Product>> GetProduct(int productid)
        {
            try
            {
                var obj = productRepository.GetProduct(productid);

                if (obj == null)
                {
                    return NotFound();
                }

                var objDto = mapper.Map<ProductDto>(obj);

                return Ok(objDto);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                ModelState.AddModelError("", $"SomeThing Went Wrong");
                return StatusCode(500, ModelState);
            }
        }

        #endregion

        #region UPDATE PRODUCT DETAILS
        [HttpPatch("{productID:int}", Name = "UpdateProduct")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int productID, [FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto == null || productID != productDto.Product_Id)
                {
                    return BadRequest(ModelState);
                }
                var productObj = mapper.Map<Product>(productDto);

                if (!productRepository.UpdateProduct(productObj))
                {
                    logger.LogTrace($"SomeThing Went Wrong Updating Product {productObj.Name}");
                    ModelState.AddModelError("", $"SomeThing Went Wrong Updating Product {productObj.Name}");
                    return StatusCode(500, ModelState);
                }

                logger.LogTrace($" Update Successfully Product :  {productObj.Name}");
                return NoContent();
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                ModelState.AddModelError("", $"SomeThing Went Wrong");
                return StatusCode(500, ModelState);
            }
        }

        #endregion

        #region CREATE PRODUCT
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        [Authorize]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productdto)
        {
            try
            {
                if (productdto == null)
                {
                    return BadRequest(ModelState);
                }

                if (productRepository.ProductExists(productdto.Name))
                {
                    ModelState.AddModelError("", "Product Exists");
                    logger.LogTrace($"Product Exists");
                    return StatusCode(404, ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var productObj = mapper.Map<Product>(productdto);

                if (!productRepository.CreateProduct(productObj))
                {
                    logger.LogTrace($"SomeThing Went Wrong Creating Product {productObj.Name}");
                    ModelState.AddModelError("", $"SomeThing Went Wrong Saving Product {productObj.Name}");
                    return StatusCode(500, ModelState);
                }
                logger.LogTrace("Create Successfull Product :" + productObj.Name);
                return CreatedAtRoute("GetProduct", new { productID = productObj.Product_Id }, productObj);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                ModelState.AddModelError("", $"SomeThing Went Wrong");
                return StatusCode(500, ModelState);
            }
        }

        #endregion

        #region DELETE PRODUCT
        [HttpDelete("{productId:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                if (!productRepository.ProductExists(productId))
                {
                    logger.LogTrace($"Product Not Found");
                    return NotFound();
                }

                var productObj = productRepository.GetProduct(productId);

                if (!productRepository.DeleteProduct(productObj))
                {
                    ModelState.AddModelError("", $"SomeThing Went Wrong Deleteing Product {productObj.Name}");
                    logger.LogTrace($"SomeThing Went Wrong Updating Product {productObj.Name}");
                    return StatusCode(500, ModelState);
                }

                logger.LogTrace($" Delete Sucessfull Product :  {productObj.Name}");
                return NoContent();
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                ModelState.AddModelError("", $"SomeThing Went Wrong");
                return StatusCode(500, ModelState);
            }
        } 
        #endregion
    }
}
