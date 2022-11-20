using System.ComponentModel.DataAnnotations;

namespace AllDailyDuties_AuthService.Models.Users
{
    public class CreateRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }


    }
}