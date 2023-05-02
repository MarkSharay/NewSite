using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRAS_Task.Models;
using PRAS_Task.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRAS_Task.Controllers
{
    public class IdentityController:Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;

        public IdentityController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IIdentityService identityService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _identityService = identityService;
        }

        [HttpPost]
        //[Route(nameof(Login))]
        public async Task<ActionResult<LoginResponse?>> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = _identityService.Login(user);
                HttpContext.Session.SetString("Token", result.Token);
                return RedirectToAction("Index", "News");
            }
            return new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        [HttpPost]
        //[Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest("User with same email already exists");

            if (model.Password != model.PasswordConfirm)
                return BadRequest("Passwords do not match");
            var user = new IdentityUser { Email = model.Email , UserName = model.Email};
            var result = await this._userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                return RedirectToAction("Index", "News");
            }
            return BadRequest(result.Errors);
        }


       [HttpGet]
        public async Task<ActionResult> GetLoginView()
        {
            return View("Login");
        }
    }
}
