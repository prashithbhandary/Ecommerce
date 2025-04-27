using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.interfaces
{
    public interface IProductService
    {
        Task<int> CreateAsync(CreateProductFormDto dto);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CreateProductFormDto dto);
        Task<List<ProductDto>> GetProductsByCategoryAsync(string category);
        Task<List<ProductDto>> SearchProductsAsync(string term);

    }
}
