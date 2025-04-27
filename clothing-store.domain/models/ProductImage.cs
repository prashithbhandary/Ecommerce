using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class ProductImage
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    public bool IsMain { get; set; } = false;

    public int Order { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
  }
}
