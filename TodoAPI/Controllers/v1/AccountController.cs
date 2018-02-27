using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoAPI.Models;
using TodoAPI.ViewModels;


using System.ComponentModel.DataAnnotations;

using System.IdentityModel.Tokens.Jwt;



using System.Security.Claims;

using System.Text;


using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
namespace TodoAPI.v1.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    //[Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,ILogger<AccountController> logger
            , IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Register()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        //[Route("/api/v1/[controller]/Register")]
        public async Task<IActionResult> Register( [FromBody] RegisterView registerView)
        {
            _logger.LogInformation($"Start registration");
            var newUser = new ApplicationUser { UserName = registerView.Email, Email = registerView.Email };
            var result = await _userManager.CreateAsync(newUser, registerView.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == registerView.Email);

                _logger.LogInformation($"User : {newUser.UserName} succesfully registered");
                return Ok(await GenerateJwtToken(registerView.Email, appUser));
                //return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
            return Ok();
        }
        
        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginView loginView)
        {
            _logger.LogInformation($"Start login user: {loginView.Email}");
            var user = new ApplicationUser { UserName = loginView.Email, Email = loginView.Email };
            
                var result = await _signInManager.PasswordSignInAsync(loginView.Email,loginView.Password,true,false);
            
            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == loginView.Email);
                _logger.LogInformation($"User : {user.UserName} succesfully logged in");
                return Ok(await GenerateJwtToken(loginView.Email, appUser));

            }
            return BadRequest("Sikertelen beelentkezés");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User : {User.ToString()} succesfully logged out");
            return Ok();
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)

        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}