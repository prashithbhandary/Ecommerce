using Microsoft.AspNetCore.Http;

namespace clothing_store.application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal? rating { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();
        public List<ProductVariantDto> Variants { get; set; } = new();
    }

    public class CreateProductFormDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public List<IFormFile> Images { get; set; } = new();
        public List<bool> IsMainList { get; set; } = new();
        public List<int> ImageOrders { get; set; } = new();

        public bool HasVariants { get; set; }
        public string VariantsJson { get; set; }

        public List<string>? ExistingImageUrls { get; set; } // ← updated!
    }


    public class ProductImageDto
    {
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }

    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string SKU { get; set; }
        public int Stock { get; set; }
    }

}
