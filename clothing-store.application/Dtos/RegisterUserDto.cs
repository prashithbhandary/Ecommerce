using System.ComponentModel.DataAnnotations;

public class RegisterUserDto
{
  [Required]
  public string FullName { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Required]
  [MinLength(6)]
  public string Password { get; set; }

  [Required]
  public string PhoneNumber { get; set; }

  public bool IsAdmin { get; set; } = false;
}
