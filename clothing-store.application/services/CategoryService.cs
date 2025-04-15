using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using clothing_store.domain.models;
using clothing_store.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.services
{
  public class CategoryService : ICategoryService
  {
    private readonly CategoryRepository _repo;

    public CategoryService(CategoryRepository repo)
    {
      _repo = repo;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
      var categories = await _repo.GetAllAsync();
      return categories.Select(c => new CategoryDto
      {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description
      });
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
      var category = await _repo.GetByIdAsync(id);
      if (category == null) return null;

      return new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
      };
    }

    public async Task<int> CreateAsync(CreateCategoryDto dto)
    {
      var category = new Category
      {
        Name = dto.Name,
        Description = dto.Description
      };
      return await _repo.CreateAsync(category);
    }

    public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
    {
      var category = await _repo.GetByIdAsync(dto.Id);
      if (category == null) return false;

      category.Name = dto.Name;
      category.Description = dto.Description;

      return await _repo.UpdateAsync(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
      return await _repo.DeleteAsync(id);
    }
  }

}
