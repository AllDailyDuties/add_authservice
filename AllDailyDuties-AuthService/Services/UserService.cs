using AllDailyDuties_AuthService.Helpers;
using AllDailyDuties_AuthService.Middleware.Authorization.Interfaces;
using AllDailyDuties_AuthService.Models.Users;
using AllDailyDuties_AuthService.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using AllDailyDuties_AuthService.Models.Users;

namespace AllDailyDuties_AuthService.Services
{
    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private IJwtUtils _jwtUtils;

        public UserService(DataContext context, IMapper mapper, IJwtUtils jwtUtils)
        {
            _context = context;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }
        public AuthResponse Authenticate(AuthRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }
        
        public User GetById(Guid id)
        {
            return getUser(id);
        }
        public void Create(CreateRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");


            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            DateTime now = DateTime.Now;

            user.Created = now;

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Helper method(s)
        private User getUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
