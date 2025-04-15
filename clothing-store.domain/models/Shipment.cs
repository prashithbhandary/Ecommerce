using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class Shipment
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    [Required]
    [MaxLength(100)]
    public string CourierCompany { get; set; }

    [MaxLength(100)]
    public string? TrackingNumber { get; set; }

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    [MaxLength(100)]
    public string? DeliveryStatus { get; set; } // Shipped, In Transit, Delivered, etc.
  }
}
