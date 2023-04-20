using ComputerShop.Data;
using ComputerShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ComputerShop.Controllers
{
	public class HomeController : Controller
	{
        private readonly ApplicationDbContext _data;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger,ApplicationDbContext data)
		{
			_data = data;
			_logger = logger;		
        }

		public IActionResult Index()
		{
			// do naprawienia (spytania w poniedziałek/wtorek)
			var categoryGroupsList = from category in _data.Categories join categoryGroup in _data.CategoryGroups on category.CategoryGroupId equals categoryGroup.Id
									 select new { CategoryName = category.Name, CategoryGroupName = categoryGroup.Name };
			return View(categoryGroupsList);
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