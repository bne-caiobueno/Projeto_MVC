using Microsoft.AspNetCore.Mvc;
using Projeto_MVC.Models;
using Npgsql;
using System.Data;
using Dapper;

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
        public IActionResult Login(string email, string password)
        {
            IDbConnection con;
            try
            {
                string getEmailQuery = $"SELECT * FROM users WHERE email = '{email}'";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                Users userLogin = con.Query(getEmailQuery).First();
                con.Close();
                if(userLogin == null)
                {
                    throw new Exception("Email não encontrado!");
                }
                if (userLogin.password != password) 
                {
                    throw new Exception("Email ou senha incorretos!");
                }

                //usuário logado
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
                    string insertQuery = "INSERT INTO users(Name,Email,Password) VALUES(@name,@email,@password)";
                    con = new NpgsqlConnection(connectionString);
                    con.Open();
                    con.Execute(insertQuery, user);
                    con.Close();
                    return RedirectToAction(nameof(Create));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return View();
        }


    }
}
