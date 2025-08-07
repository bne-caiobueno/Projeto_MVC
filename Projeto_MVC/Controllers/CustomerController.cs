using Microsoft.AspNetCore.Mvc;

namespace Projeto_MVC.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
