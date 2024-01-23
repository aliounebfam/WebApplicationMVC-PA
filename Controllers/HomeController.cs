using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
	public class HomeController(ILogger<HomeController> logger, DBConnect dbConnection) : Controller
	{
		private readonly ILogger<HomeController> _logger = logger;
        private readonly DBConnect _dbConnection = dbConnection;

        public IActionResult Index()
        {
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

        public IActionResult Departement()
        {
            return View();
        }

        public IActionResult Filiere()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
