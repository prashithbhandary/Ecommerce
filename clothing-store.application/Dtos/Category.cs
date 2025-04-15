using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.Dtos
{
  public class CategoryDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
  }

  public class CreateCategoryDto
  {
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string? Description { get; set; }
  }

  public class UpdateCategoryDto
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string? Description { get; set; }
  }

}
