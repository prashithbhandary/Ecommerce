using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clothing_store.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductFormDto dto)
        {
            var id = await _productService.CreateAsync(dto);
            return Ok(new { productId = id, message = "Product created successfully" });
        }

        [HttpGet("get-all-product")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("product-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductFormDto dto)
        {
            var success = await _productService.UpdateAsync(id, dto);
            if (!success) return NotFound();

            return Ok(new { message = "Product updated successfully" });
        }

        [HttpGet("products-by-category/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string term)
        {
            var products = await _productService.SearchProductsAsync(term);
            return Ok(products);
        }

    }
}
