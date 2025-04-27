using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using clothing_store.domain.models;
using clothing_store.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace clothing_store.application.services
{
    public class ProductService : IProductService
    {
        private readonly IBlobService _blobService;
        private readonly ProductRepository _productRepo;

        public ProductService(IBlobService blobService, ProductRepository productRepo)
        {
            _blobService = blobService;
            _productRepo = productRepo;
        }

        public async Task<int> CreateAsync(CreateProductFormDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
            };

            for (int i = 0; i < dto.Images.Count; i++)
            {
                var file = dto.Images[i];
                var imageUrl = await _blobService.UploadFileAsync(file);

                product.Images.Add(new ProductImage
                {
                    ImageUrl = imageUrl,
                    IsMain = dto.IsMainList.Count > i && dto.IsMainList[i],
                    Order = i
                });;
            }

            if (dto.HasVariants && !string.IsNullOrEmpty(dto.VariantsJson))
            {
                var variants = JsonSerializer.Deserialize<List<ProductVariantDto>>(dto.VariantsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                foreach (var v in variants)
                {
                    product.Variants.Add(new ProductVariant
                    {
                        Size = v.Size,
                        Color = v.Color,
                        SKU = v.SKU,
                        Stock = v.Stock
                    });
                }
            }

            return await _productRepo.CreateAsync(product);
        }


        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepo.GetAllWithIncludesAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                Images = p.Images.Select(i => new ProductImageDto
                {
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList(),
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Size = v.Size,
                    Color = v.Color,
                    SKU = v.SKU,
                    Stock = v.Stock
                }).ToList()
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdWithIncludesAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                Images = product.Images.Select(i => new ProductImageDto
                {
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList(),
                Variants = product.Variants.Select(v => new ProductVariantDto
                {
                    Id= v.Id,
                    Size = v.Size,
                    Color = v.Color,
                    SKU = v.SKU,
                    Stock = v.Stock
                }).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateProductFormDto dto)
        {
            var product = await _productRepo.GetByIdWithIncludesAsync(id);
            if (product == null) return false;

            // Basic product fields update
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;

            // Normalize DTO image URLs
            var normalizedExistingUrlsSet = new HashSet<string>(
                dto.ExistingImageUrls?
                    .Select(url => url.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormC))
                    ?? Enumerable.Empty<string>(),
                StringComparer.OrdinalIgnoreCase);

            // Identify images to remove
            var imagesToRemove = product.Images
                .Where(img => !normalizedExistingUrlsSet.Contains(img.ImageUrl.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormC)))
                .ToList();

            foreach (var img in imagesToRemove)
            {
                await _blobService.DeleteFileAsync(img.ImageUrl);
                product.Images.Remove(img);
            }

            if (dto.ExistingImageUrls != null)
            {
                for (int i = 0; i < dto.ExistingImageUrls.Count; i++)
                {
                    var url = dto.ExistingImageUrls[i];
                    var img = product.Images.FirstOrDefault(x => x.ImageUrl == url);
                    if (img != null)
                    {
                        int combinedIndex = i; // existing image index in combined list
                        img.IsMain = dto.IsMainList[combinedIndex];
                        img.Order = dto.ImageOrders[combinedIndex];
                    }
                }
            }

            // Add newly uploaded images
            int newImagesStartIndex = dto.ExistingImageUrls?.Count ?? 0;

            for (int i = 0; i < dto.Images.Count; i++)
            {
                var combinedIndex = newImagesStartIndex + i;
                var imageUrl = await _blobService.UploadFileAsync(dto.Images[i]);

                product.Images.Add(new ProductImage
                {
                    ImageUrl = imageUrl,
                    IsMain = dto.IsMainList[combinedIndex],
                    Order = dto.ImageOrders[combinedIndex]
                });
            }

            // Update variants
            product.Variants.Clear();

            if (dto.HasVariants && !string.IsNullOrEmpty(dto.VariantsJson))
            {
                var variants = JsonSerializer.Deserialize<List<ProductVariantDto>>(dto.VariantsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                foreach (var v in variants)
                {
                    product.Variants.Add(new ProductVariant
                    {
                        Size = v.Size,
                        Color = v.Color,
                        SKU = v.SKU,
                        Stock = v.Stock
                    });
                }
            }

            await _productRepo.UpdateAsync(product);
            return true;
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productRepo.GetProductsByCategoryAsync(category);

            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                Images = product.Images.Select(i => new ProductImageDto
                {
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList(),
                Variants = product.Variants.Select(v => new ProductVariantDto
                {Id = v.Id,
                    Size = v.Size,
                    Color = v.Color,
                    SKU = v.SKU,
                    Stock = v.Stock
                }).ToList()
            }).ToList();
        }

        public async Task<List<ProductDto>> SearchProductsAsync(string term)
        {
            term = term.ToLower();

            var products = await _productRepo.SearchProductsAsync(term);

            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                Images = product.Images.Select(i => new ProductImageDto
                {
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList(),
                Variants = product.Variants.Select(v => new ProductVariantDto
                {Id = v.Id,
                    Size = v.Size,
                    Color = v.Color,
                    SKU = v.SKU,
                    Stock = v.Stock
                }).ToList()
            }).ToList();
        }


    }

}
