using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Entities2;
using WebApplication1.Models;
using System.Data;
using System.Security.Principal;
using Serilog.Core;
using NuGet.Protocol.Core.Types;
using Serilog;
using System.Net;
using Dadata.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Entities2;

namespace WebApplication1.Controllers
{
    public class AccauntController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DiplomContext _context;

        public AccauntController(ILogger<HomeController> logger, DiplomContext context)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public static string HashString(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем входную строку в байты и вычисляем хэш
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Конвертируем байты в шестнадцатеричную строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public IActionResult Profile()
        {
            ProfileViewModel model = new ProfileViewModel();
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(x => x.Name == username);
            model.user = user;
            model.orders = _context.Orders.Include(d=>d.Status).Where(d => d.UserId == user.Id).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login (LoginViewModel model)
        {
          
            if (ModelState.IsValid)
            {

                var findUser = _context.Users.FirstOrDefault(d => d.Login == model.Login && d.Password == HashString( model.Password));
                if (findUser ==null)
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                    return View("Index", model);
                }
            try
            {

                

                    Log.Information($"Пользователь {findUser.Login} успешно вошел в систему в систему");

                    List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, findUser.Name),
                    new Claim(ClaimTypes.Role, findUser.Role),
                    new Claim(ClaimTypes.NameIdentifier, findUser.Login)
    };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");

               
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Ошибка входа ");
                return View();
            }
            
            }
            else
            {
                 return View("Index",model);
            }
            
        }
        
        public IActionResult Registration(RegistrarionViewModel model)
        {
            

           return View();
            
        }
        [HttpPost]
        public IActionResult NewRegistration(RegistrarionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.RePassword)
                {
                    ModelState.AddModelError(string.Empty, "Пароли не совпадают");
                    return View("Registration", model);
                }
            var user = new User();
            user.Name = model.Name;
            user.Login =  model.Login;
            user.Password = HashString(model.Password);
            user.Role = "user";
            _context.Users.Add(user);
            _context.SaveChanges();

            var findUser = user;
            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, findUser.Name),
                    new Claim(ClaimTypes.Role, findUser.Role),
                    new Claim(ClaimTypes.NameIdentifier, findUser.Login)
    };
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");

            }
            else
            {
                return View("Registration",model);
            }
        }
    }
}
