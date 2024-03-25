using System.ComponentModel.DataAnnotations;

namespace CoreInfra.Shared.Models
{
    public class RegisterModel 
    {
        public string? ClientID { get; set; }
        public string? First6DigitsOfCard { get; set; }
        public string? Last4DigitsOfCard { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "the password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
