using ComputerShop.Data;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace ComputerShop.Components
{
    public class NavigationBarViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public NavigationBarViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
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
            return View("_NavCategoryBar",model);
        }
    }
}
