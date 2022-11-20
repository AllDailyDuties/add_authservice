using System.ComponentModel.DataAnnotations;

namespace AllDailyDuties_AuthService.Models.Users
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
