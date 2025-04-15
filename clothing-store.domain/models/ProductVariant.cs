using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class ProductVariant
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Size { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }

    public int Stock { get; set; }

    [Required]
    [MaxLength(100)]
    public string SKU { get; set; }


    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
  }
}
