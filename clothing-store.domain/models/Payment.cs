using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_store.domain.models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        [MaxLength(50)]
        public string Method { get; set; } // e.g., Razorpay, UPI, etc.

        public bool IsPaid { get; set; } = false;

        public DateTime? PaidAt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AmountPaid { get; set; }

        // Razorpay payment_id
        [MaxLength(100)]
        public string? TransactionId { get; set; }
    }
}
