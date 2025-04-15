using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_store.domain.models
{
  public class OrderItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    [Required]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public int? ProductVariantId { get; set; }
    public virtual ProductVariant? Variant { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
  }
}
