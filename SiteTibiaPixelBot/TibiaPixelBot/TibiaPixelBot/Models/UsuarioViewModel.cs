using System.ComponentModel.DataAnnotations;

namespace TibiaPixelBot.Models
{
    public class UsuarioViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Message { get; set; }
    }
}
