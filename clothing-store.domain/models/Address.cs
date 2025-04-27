using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string name { get; set; }

    [Required]
  [MaxLength(255)]
  public string addressLine1 { get; set; }

  [MaxLength(255)]
  public string? addressLine2 { get; set; }

  [Required]
  [MaxLength(100)]
  public string City { get; set; }

  [Required]
  [MaxLength(100)]
  public string State { get; set; }

  [Required]
  [MaxLength(20)]
  public string PostalCode { get; set; }

  [Required]
  [MaxLength(100)]
  public string Country { get; set; }

  // Foreign key reference to ApplicationUser
  [Required]
  public int UserId { get; set; }

  public bool IsPrimary { get; set; } = false;

  [ForeignKey("UserId")]
  public virtual ApplicationUser User { get; set; }

}
