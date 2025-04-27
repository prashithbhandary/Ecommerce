using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clothing_store.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
  {
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    [HttpGet("getallcategory")]
    public async Task<IActionResult> GetAll()
    {
      var result = await _categoryService.GetAllAsync();
      return Ok(result);
    }

    [HttpGet("getcategorybyid/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var category = await _categoryService.GetByIdAsync(id);
      if (category == null) return NotFound();
      return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("addcategory")]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
      var id = await _categoryService.CreateAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("updatecategory")]
    public async Task<IActionResult> Update(UpdateCategoryDto dto)
    {
      var success = await _categoryService.UpdateAsync(dto);
      if (!success) return NotFound();
      return Ok(new { message = "Category updated" });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deletecategory/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var success = await _categoryService.DeleteAsync(id);
      if (!success) return NotFound();
      return Ok(new { message = "Category deleted" });
    }
  }
}
