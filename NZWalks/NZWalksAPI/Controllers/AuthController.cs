using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        //api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto )
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await _userManager
                .CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                   identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                   if(identityResult.Succeeded)
                   {
                        return Ok("User was registered, please login");
                   }
                }
            }
            return BadRequest("Something went wrong");
        }

        //api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
            if(user != null)
            {
                var confirmUser = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(confirmUser)
                {
                    //Get Roles for this user
                    var roles = await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        //create token
                        var jwtToken =  _tokenRepository.CreateJwtToken(user,roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                       return Ok(response);
                    }
                }
            }
            return BadRequest("Username or password is not correct");
        }
    } 
}
