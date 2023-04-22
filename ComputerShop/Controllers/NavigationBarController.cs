using ComputerShop.Data;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShop.Controllers
{
    public class NavigationBarController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NavigationBarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<HomeViewModel> model = new List<HomeViewModel>();
            foreach (var item in _context.CategoryGroups)
            {
                model.Add(new HomeViewModel
                {
                    CategoryGroup = item,
                    Categories = _context.Categories.Where(x => x.CategoryGroupId == item.Id)
                });
            }

            return PartialView("_NavigationBarPartial", model);
        }
    }
}
