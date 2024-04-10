using Microsoft.AspNetCore.Mvc;
using Core.IServices;
using WebRenderer.Models;

namespace WebRenderer.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View("~/Views/User/Login.cshtml");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate credentials (hashed password is passed)
                if (_userService.Authenticate(model.Username, model.Password))
                {
                    // Authentication successful, generate JWT token
                    var token = _userService.GenerateJwtToken(model.Username);
                    
                    var siteBUrl = _configuration["SiteBUrl"];

                    return Redirect($"{siteBUrl}?token={token}");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View("~/Views/User/Login.cshtml", model);
                }
            }
            return View("~/Views/User/Login.cshtml", model);
        }

        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View("~/Views/User/Register.cshtml");
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Register the user
                    _userService.Register(model.Username, model.Password);
                    return RedirectToAction("Login");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View("~/Views/User/Register.cshtml", model);
        }
    }
}
