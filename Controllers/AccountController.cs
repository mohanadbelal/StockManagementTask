using Assignment.Task.Data;
using Assignment.Task.Dtos;
using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Security.Claims;

namespace Assignment.Task.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContextDapper _dapper;

        private readonly AuthHelpers _authHelpers;

        private readonly NLog.Logger _logger = NLog.LogManager.GetLogger(nameof(AccountController));


        public AccountController(IConfiguration iConfig)
        {
            _dapper = new DataContextDapper(iConfig);

            _authHelpers = new AuthHelpers(iConfig);

        }



        public IActionResult Login()
        {
            
            Response.Cookies.Delete("JwtToken");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var PasswordHash = _authHelpers.GetPasswordHash(password);

            User currUser;

            _logger.Info("Login attempt for user {0}", username);

            if(_authHelpers.LoginUser(username, PasswordHash , out currUser ))
            {
                string JwtToken = _authHelpers.CreateToken(currUser.Id, currUser.Role);

            
                Response.Cookies.Append("JwtToken", JwtToken, new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                });

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                _logger.Warn("Invalid login attempt for user {0}", username);
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                return View();
            }
        }

        public IActionResult Logout()
        {
            _logger.Info("Logging out and deleting JwtToken cookie. UserId : " + User.FindFirstValue("userId"));
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            _logger.Info("Showing registration page.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto model)
        {
            if (model.Password != model.passwordConfirm)
            {
                _logger.Warn("Registration attempt where passwords do not match for username {0} , Password {1} , Password Confirm {2}", model.Username,model.Password,model.passwordConfirm);
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(model);
            }

            if (model.Password.Length < 8)
            {
                ModelState.AddModelError(string.Empty, "Password can't be less 8 characters");
                return View(model);
            }

            string sql = "SELECT Username  FROM [StockManagementDb].[dbo].[User] where Username ='"
                    + model.Username + "'";
            IEnumerable<string> existingUsers = _dapper.LoadData<string>(sql);

            if (existingUsers.Count() == 0)
            {

                var PasswordHash = _authHelpers.GetPasswordHash(model.Password);

                User newUser = new User()
                {
                    Username = model.Username,
                    PasswordHash = PasswordHash,
                    Role = "Normal"
                };

                if (_authHelpers.RegisterUser(newUser))
                {
                    _logger.Info("User {0} registered successfully", model.Username);
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    _logger.Error("Failed to create user {0}", model.Username);
                    ModelState.AddModelError(string.Empty, "Failed to Create");
                    return View(model);
                }
            }
            else
            {
                _logger.Warn("Registration attempt with existing username {0}", model.Username);
                ModelState.AddModelError(string.Empty, "Username already exist, Choose another one");
                return View(model);
            }
            
            
        }
    }
}
