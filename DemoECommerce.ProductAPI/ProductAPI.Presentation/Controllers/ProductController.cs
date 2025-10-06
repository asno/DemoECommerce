using ECommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTO;
using ProductAPI.Application.DTO.Conversions;
using ProductAPI.Application.Interface;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No product found.");
            }

            var (_, list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No product found.");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProducts(int id)
        {
            var product = await productInterface.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not found.");
            }

            var (_product, _) = ProductConversion.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound("Product not found.");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = ProductConversion.ToEntity(productDTO);
            var response = await productInterface.UpdateAsync(product);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = ProductConversion.ToEntity(productDTO);
            var response = await productInterface.DeleteAsync(product);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = ProductConversion.ToEntity(productDTO);
            var response = await productInterface.CreateAsync(product);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
