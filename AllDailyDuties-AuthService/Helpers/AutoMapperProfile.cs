using AllDailyDuties_AuthService.Models.Users;
using AutoMapper;

namespace AllDailyDuties_AuthService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateRequest -> User
            CreateMap<CreateRequest, User>();
            CreateMap<User, AuthResponse>();
        }
    }
}
