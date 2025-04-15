using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class Cart
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
  }
}
