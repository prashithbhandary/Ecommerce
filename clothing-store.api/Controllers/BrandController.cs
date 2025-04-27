using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clothing_store.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("getallbrand")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("getbrandbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addbrand")]
        public async Task<IActionResult> Create(CreateBrandDto dto)
        {
            var id = await _brandService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updatebrand")]
        public async Task<IActionResult> Update(UpdateBrandDto dto)
        {
            var success = await _brandService.UpdateAsync(dto);
            if (!success) return NotFound();
            return Ok(new { message = "Category updated" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deletebrand/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _brandService.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Category deleted" });
        }
    }
}
