using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class ApplicationUser 
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string FullName { get; set; }

  [Required]
  [EmailAddress]
  [MaxLength(255)]
  public string Email { get; set; } 

  [Required]
  [MaxLength(20)]
  public string PhoneNumber { get; set; } 

  [Required]
  public string PasswordHash { get; set; }

  public bool IsAdmin { get; set; } = false;

  public int AccessFailedCount { get; set; } = 0;

  public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
