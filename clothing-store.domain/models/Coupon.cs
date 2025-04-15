using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class Coupon
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [Required]
    public bool IsPercentage { get; set; } // true: % off, false: flat discount

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int? MaxUsageCount { get; set; } // optional global limit

    public bool IsActive { get; set; } = true;

    public virtual ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
  }
}
