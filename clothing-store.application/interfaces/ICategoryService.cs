using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.interfaces
{
  public interface ICategoryService
  {
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateCategoryDto dto);
    Task<bool> UpdateAsync(UpdateCategoryDto dto);
    Task<bool> DeleteAsync(int id);
  }
}
