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

namespace TodoAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    //[Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        //[Authorize(AuthenticationSchemes = "Identity.Application")]
        [AllowAnonymous]
        // [Route("/api/v1/[controller]/Register")]
        public IActionResult Register()
        {
            return Ok("sajt");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("/api/[controller]/Register")]
        public async Task<IActionResult> Register( [FromBody] RegisterView registerView)
        {
            _logger.LogInformation($"Start registration");
            var newUser = new ApplicationUser { UserName = registerView.Email, Email = registerView.Email };
            var result = await _userManager.CreateAsync(newUser, registerView.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                _logger.LogInformation($"User : {newUser.UserName} succesfully registered");
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
            return Ok();
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //[Route("/api/[controller]/Login")]
        public async Task<IActionResult> Login([FromBody] LoginView loginView)
        {
            _logger.LogInformation($"Start login user: {loginView.Email}");
            var user = new ApplicationUser { UserName = loginView.Email, Email = loginView.Email };
            
                var result = await _signInManager.PasswordSignInAsync(loginView.Email,loginView.Password,true,false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation($"User : {user.UserName} succesfully logged in");
                return Ok();
            }
            return BadRequest("Sikertelen beelentkezés");
        }
        [HttpPost]
        //[Route("/api/[controller]/Logout")]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User : {User.ToString()} succesfully logged out");
            return Ok();
        }

        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //    var identity = await UserManager.CreateIdentityAsync(
        //       user, DefaultAuthenticationTypes.ApplicationCookie);

        //    AuthenticationManager.SignIn(
        //       new AuthenticationProperties()
        //       {
        //           IsPersistent = isPersistent
        //       }, identity);
        //}
        [HttpGet]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //[Route("/api/[controller]/Login")]
        public async Task<IActionResult> Login()
        {
            return Ok("Rossz felhasználónév");
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return Ok("Acces denied");
        }
    }
}