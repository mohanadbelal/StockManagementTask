using Assignment.Task.Data;
using Assignment.Task.Dtos;
using Assignment.Task.Helpers;
using Assignment.Task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace Assignment.Task.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContextDapper _dapper;

        private readonly AuthHelpers _authHelpers;

        private readonly SqlHelper _sqlHelper;

        public AccountController(IConfiguration iConfig)
        {
            _dapper = new DataContextDapper(iConfig);

            _authHelpers = new AuthHelpers(iConfig);

            _sqlHelper = new SqlHelper(iConfig);
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var PasswordHash = _authHelpers.GetPasswordHash(password);

            User currUser;

            if(_sqlHelper.LoginUser(username, PasswordHash , out currUser ))
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
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                return View();
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto model)
        {
            if (model.Password != model.passwordConfirm)
            {
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

                if (_sqlHelper.RegisterUser(newUser))
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to Create");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username already exist, Choose another one");
                return View(model);
            }
            // TODO: Implement user registration logic
            
        }
    }
}
