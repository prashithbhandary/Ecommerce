using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class Order
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Paid, Shipped, Delivered, Cancelled

    public int AddressId { get; set; }
    public virtual Address Address { get; set; }

    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public virtual Payment Payment { get; set; }
    public virtual Shipment Shipment { get; set; }
  }
}
