using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Npgsql;
using Projeto_MVC.Models;
using System.Data;
using System.Security.Claims;

namespace Projeto_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly string connectionString = "Host=localhost;Database=db_mvc;User ID=postgres;Password=123";
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            IDbConnection con;
            try
            {
                string getEmailQuery = $"SELECT * FROM users WHERE email = '{user.email}'";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                Users userLogin = con.Query<Users>(getEmailQuery).FirstOrDefault();
                con.Close();
                if(userLogin == null)
                {
                    TempData["msg"] = "Email não encontrado!";
                    return View();
                }
                if (userLogin.password != user.password) 
                {
                    TempData["msg"] = "Email ou senha incorretos!";
                    return View();
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userLogin.name),
                new Claim("UserType", "Admin")
            };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
                return RedirectToAction("Index", "Customer");
            }
            catch (Exception ex)
            {
                 throw ex;
            }

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Users user)
        {
            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    string insertQuery = "INSERT INTO users(name,email,password) VALUES(@name,@email,@password)";
                    con = new NpgsqlConnection(connectionString);
                    con.Open();
                    con.Execute(insertQuery, user);
                    con.Close();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}
