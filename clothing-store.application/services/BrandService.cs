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
    public class BrandService : IBrandService
    {
        private readonly BrandRepository _repo;

        public BrandService(BrandRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brand = await _repo.GetAllAsync();
            return brand.Select(c => new BrandDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand == null) return null;

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };
        }

        public async Task<int> CreateAsync(CreateBrandDto dto)
        {
            var brand = new Brand
            {
                Name = dto.Name,
                Description = dto.Description
            };
            return await _repo.CreateAsync(brand);
        }

        public async Task<bool> UpdateAsync(UpdateBrandDto dto)
        {
            var brand = await _repo.GetByIdAsync(dto.Id);
            if (brand == null) return false;

            brand.Name = dto.Name;
            brand.Description = dto.Description;

            return await _repo.UpdateAsync(brand);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
