using ComputerShop.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ComputerShop.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int dataId)
        {
            var productsToShow = _context.Products.Where(x => x.CategoryId == dataId).ToList();
            return View(productsToShow);
        }
    }
}
