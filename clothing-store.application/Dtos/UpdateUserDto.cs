using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.Dtos
{
  public class UpdateUserDto
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
  }

}
