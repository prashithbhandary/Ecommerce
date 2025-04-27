using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateBrandDto dto);
        Task<bool> UpdateAsync(UpdateBrandDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
