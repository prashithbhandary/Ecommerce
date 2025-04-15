using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
  public class CouponUsage
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CouponId { get; set; }
    public virtual Coupon Coupon { get; set; }

    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    public DateTime UsedOn { get; set; } = DateTime.UtcNow;
  }
}
