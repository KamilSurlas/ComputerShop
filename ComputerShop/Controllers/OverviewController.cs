using ComputerShop.Data;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public IActionResult Index(int? dataId)
        {           
            var searchingText = HttpContext.Request.Query["searchingText"];
            OverviewPageViewModel overviewPageViewModel = new OverviewPageViewModel();
            List<Models.Product> productsToShow;        
            if (!string.IsNullOrEmpty(searchingText))
            {
                productsToShow = _context.Products.Include(x => x.Producer).Where(x => x.Name.Contains(searchingText)).ToList();
            }
            else
            {
                productsToShow = _context.Products.Include(x => x.Producer).Where(x => x.CategoryId == dataId).ToList();
            }
            foreach (var item in productsToShow)
            {
                if(item.Producer.IsPromoted == true)
                {
                    overviewPageViewModel.PromotedProducts.Add(item);
                } else
                {
                    overviewPageViewModel.Products.Add(item);
                }
            }
            return View(overviewPageViewModel);
            
        }

    }
}
