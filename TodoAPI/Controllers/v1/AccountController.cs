using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.ViewModels;

namespace TodoAPI.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Authorize]
        [Route("/api/v1/[controller]/Register")]
        public IActionResult Register()
        {
            return Ok("sajt");
        }

        [HttpPost]
        [Route("/api/v1/[controller]/Register")]
        public async Task<IActionResult> Register( [FromBody] RegisterView registerView)
        {
            var newUser = new ApplicationUser { UserName = registerView.Email, Email = registerView.Email };
            var result = await _userManager.CreateAsync(newUser, registerView.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
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
        [Route("/api/v1/[controller]/Login")]
        public async Task<IActionResult> Login([FromBody] LoginView loginView)
        {
            var user = new ApplicationUser { UserName = loginView.Email, Email = loginView.Email };
            
                var result = await _signInManager.PasswordSignInAsync(loginView.Email,loginView.Password,true,false);
            
            if (result.Succeeded)
                return Ok();
            return BadRequest("Sikertelen beelentkezés");
        }
        [HttpPost]
        [Route("/api/v1/[controller]/Logout")]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
          
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
    }
}