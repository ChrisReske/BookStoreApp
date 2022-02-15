using AutoMapper;
using BookStoreAppApi.Data;
using BookStoreAppApi.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;


        public AuthController(
            ILogger<AuthController> logger,
            IMapper mapper, 
            UserManager<ApiUser> userManager)
        {
            _logger = logger
                      ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper
                      ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager 
                           ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> Register(UserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email} ");

            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;

                var result = await _userManager.CreateAsync(user, userDto.Password);
            
                if (result.Succeeded == false)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            
                await _userManager.AddToRoleAsync(user, "User");
                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(ReaderWriterLock)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            _logger.LogInformation($"Login attempt for {loginUserDto.Email}");
            try
            {
                var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

                if(user == null || isPasswordValid == false)
                {
                    return NotFound();
                }

                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(ReaderWriterLock)}", statusCode: 500);

            }
        }

    }
}
