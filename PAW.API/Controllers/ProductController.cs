using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ILogger<ProductController> logger, IProductRepository productRepository) : ControllerBase
    {
        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = await productRepository.ReadAsync() ?? [];
            return products.Select(ProductDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await productRepository.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(ProductDTO.ConvertFrom(product));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] ProductDTO productDTO)
        {
            var product = ProductDTO.ConvertTo(productDTO);
            product.ProductId = 0; // Ensure the ProductId is set to 0 for new products
            product.LastModified = DateTime.Now;
            var result = await productRepository.CreateAsync(product);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var product = await productRepository.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var result = await productRepository.DeleteAsync(product);
            return Ok(result);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] ProductDTO productDTO)
        {
            var product =
                await productRepository.FindAsync(id);

            if (product == null)
                return NotFound();


            product.ProductName =
                productDTO.Name;

            product.Description =
                productDTO.Description;

            product.Rating =
                productDTO.Rating;

            product.CategoryId =
                productDTO.CategoryId;

            product.SupplierId =
                productDTO.SupplierId;

            product.InventoryId =
                productDTO.InventoryId;

            product.CreatedBy =
                productDTO.CreatedBy;

            product.ModifiedBy =
                productDTO.ModifiedBy;

            product.LastModified =
                DateTime.Now;


            var result =
                await productRepository.UpdateAsync(product);

            return Ok(result);
        }

    }
}
 