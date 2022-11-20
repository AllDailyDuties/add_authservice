using System.Text.Json.Serialization;
namespace AllDailyDuties_AuthService.Models.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        public DateTime Created { get; set; }

    }
}

