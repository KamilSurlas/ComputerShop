using Microsoft.AspNetCore.Mvc;
using ComputerShop.Models;
using System.Diagnostics;
using ComputerShop.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerShop.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int productId)
        {
            var product = _context.Products.Include(x => x.Producer).Where(x => x.Id == productId).Include(x=>x.ImagesUrls).Where(x=>x.Id == productId).FirstOrDefault();     
            return View(product);
        }
    }
}
