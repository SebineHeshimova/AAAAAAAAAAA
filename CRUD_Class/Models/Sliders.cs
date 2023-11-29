using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Class.Models;

public class Sliders:BaseEntity
{     
    [Required]
    [StringLength(maximumLength: 30)]
    public string Title { get; set; }
    [Required]
    [StringLength(maximumLength: 100)]
    public string Description { get; set; }
    [StringLength(maximumLength: 100)]
    public string? ImageUrl { get; set; }
    [Required]
    public string RedirectUrl { get; set; }
    [Required]
    [StringLength(maximumLength: 30)]
    public string RedirectText { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
