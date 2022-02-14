using AutoMapper;
using BookStoreAppApi.Data;
using BookStoreAppApi.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(
            ILogger<AuthController> logger, 
            IMapper mapper)
        {
            _logger = logger 
                      ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper 
                      ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);

            return Content("");
        }

        //public async Task<IActionResult> Login(string returnUrl)

    }
}
