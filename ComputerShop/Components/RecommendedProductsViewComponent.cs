using ComputerShop.Data;
using ComputerShop.Models;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerShop.Components
{
    public class RecommendedProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public RecommendedProductsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            List<Product> list = new List<Product>();
            list = _context.Products.Include(x=>x.Producer).Where(x=>x.Producer.IsPromoted).ToList();
            return View("_RecommendedProducts",list);
        }
    }
}
