using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_store.domain.models
{
  public class CartItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CartId { get; set; }
    public virtual Cart Cart { get; set; }

    [Required]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public int? ProductVariantId { get; set; }
    public virtual ProductVariant? Variant { get; set; }


    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
  }
}
