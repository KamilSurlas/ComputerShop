using ComputerShop.Data;
using ComputerShop.Models;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ComputerShop.Controllers
{
	public class HomeController : Controller
	{
        private readonly ApplicationDbContext _data;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext data)
		{
			_data = data;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
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