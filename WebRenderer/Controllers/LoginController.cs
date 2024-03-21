using Microsoft.AspNetCore.Mvc;
using Core.IServices;
using WebRenderer.Models;

namespace WebRenderer.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IUserService _userService; // Add UserService

        public LoginController(ILoginService loginService, IUserService userService) // Inject UserService
        {
            _loginService = loginService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Hash the provided password UserService
                string hashedPassword = _userService.HashPassword(model.Password);

                // Validate credentials (hashed password is passed)
                if (_loginService.ValidateCredentials(model.Username, hashedPassword))
                {
                    // Authentication successful, generate JWT token
                    var token = _loginService.GenerateJwtToken(model.Username);
                    return Redirect($"https://othersite.com?token={token}");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View("Index", model);
                }
            }
            return View("Index", model);
        }
    }
}
