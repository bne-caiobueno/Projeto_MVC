using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Projeto_MVC.Models;
using System.Data;
using System.Data.Common;

namespace Projeto_MVC.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly string connectionString = "Host=localhost;Database=db_mvc;User ID=postgres;Password=123";
        
        [HttpGet]
        public IActionResult Index()
        {
            IDbConnection con;
            try
            {
                string CustomerQuery = $"SELECT * FROM customers";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                IList<Customers> customers = con.Query<Customers>(CustomerQuery).ToList();
                con.Close();

                return View(customers);
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
        public IActionResult Create(Customers customer)
        {
            IDbConnection con;

            try
            {
                string insertCustomer = "INSERT INTO customers(name,job,age) VALUES(@name,@job,@age)";
                con = new NpgsqlConnection(connectionString); 
                con.Open();
                con.Execute(insertCustomer, customer);
                con.Close();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            IDbConnection con;
            try
            {
                string customerQuery = $"SELECT * FROM customers WHERE id = {id}";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                Customers customer = con.Query<Customers>(customerQuery).FirstOrDefault();
                con.Close();
                if (customer == null)
                {
                    throw new Exception("Cliente não encontrado!");
                }

                return View(customer);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            IDbConnection con;
            try
            {
                string deleteCustomer = $"DELETE FROM customers WHERE id = @id";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                con.Execute(deleteCustomer, new { id = id });
                con.Close();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            IDbConnection con;
            try
            {
                string customerQuery = $"SELECT * FROM customers WHERE id = {id}";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                Customers customer = con.Query<Customers>(customerQuery).FirstOrDefault();
                if(customer == null)
                {
                    throw new Exception("Cliente não encontrado!");
                }
                return View(customer);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Edit(Customers customer)
        {
            IDbConnection con;
            try
            {
                string updateCustomer = "UPDATE customers SET name=@name,job=@job,age=@age WHERE id = @id";
                con = new NpgsqlConnection(connectionString);
                con.Open();
                con.Execute(updateCustomer, customer);
                con.Close();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
