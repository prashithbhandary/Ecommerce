using System.ComponentModel.DataAnnotations;

public class LoginUserDto
{
  [Required]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }
}
