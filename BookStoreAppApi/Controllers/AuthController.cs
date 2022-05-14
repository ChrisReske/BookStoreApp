﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BookStoreAppApi.Data;
using BookStoreAppApi.Models.User;
using BookStoreAppApi.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookStoreAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            ILogger<AuthController> logger,
            IMapper mapper,
            UserManager<ApiUser> userManager, 
            IConfiguration configuration)
        {
            _logger = logger
                      ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper
                      ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager
                           ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration 
                             ?? throw new ArgumentNullException(nameof(configuration));
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
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto loginUserDto)
        {
            _logger.LogInformation($"Login attempt for {loginUserDto.Email}");
            try
            {
                var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

                if(user == null || isPasswordValid == false)
                {
                    return Unauthorized(loginUserDto);
                }

                var tokenString = await GenerateToken(user);

                var response = new AuthResponse
                {
                    Email = loginUserDto.Email,
                    Token = tokenString,
                    UserId = user.Id,
                };

                return Accepted(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(ReaderWriterLock)}", statusCode: 500);

            }
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q));

            // Add claims from database
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id),
            }
                .Union(userClaims)
                .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}