using AllDailyDuties_AuthService.Middleware.Authorization.Interfaces;
using AllDailyDuties_AuthService.Models.Users;
using AllDailyDuties_AuthService.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllDailyDuties_AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;
        private IMapper _mapper;
        private IJwtUtils _jwtUtils;
        public UserController(ILogger<UserController> logger, IUserService userService,
        IMapper mapper, IJwtUtils jwtUtils)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }
        [HttpPost("login")]
        public IActionResult Login(AuthRequest model)
        {
            var loginResult = _userService.Authenticate(model);
            return Ok(loginResult);

        }
        [HttpGet("verifyLogin")]
        public IActionResult VerifyLogin()
        {
            var authHeader = Request.HttpContext?.Request.Headers.Authorization;
            if (string.IsNullOrEmpty(authHeader?.ToString()))
                return Ok(false);

            var jwt = authHeader?.ToString().Replace("Bearer ", "");


            if (string.IsNullOrEmpty(jwt))
                return Ok(false);

            var user =  _jwtUtils.ValidateToken(jwt);
            if (user is null)
                return Ok(false);

            // Valid JWT
            return Ok(user);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        [HttpPost]
        public IActionResult Create(CreateRequest model)
        {
            _userService.Create(model);
            return Ok(new { message = "User created" });
        }
    }
}