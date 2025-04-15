using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class Product
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    [ForeignKey("Brand")]
    public int BrandId { get; set; }
    public virtual Brand Brand { get; set; }

    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
  }

}
