using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllUpFTB.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Title {  get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string TitleSpan {  get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Description { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string PromotingDesc {  get; set; }
        [StringLength(maximumLength: 100)]
        public string? ImageUrl {  get; set; }
        public string RedirecUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
