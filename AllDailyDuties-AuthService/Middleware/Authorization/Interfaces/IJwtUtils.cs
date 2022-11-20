using AllDailyDuties_AuthService.Models.Users;

namespace AllDailyDuties_AuthService.Middleware.Authorization.Interfaces
{
    public interface IJwtUtils
    {
        public string GenerateToken(User user);
        public Guid? ValidateToken(string token);
    }
}
