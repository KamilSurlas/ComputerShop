using ComputerShop.Data;
using ComputerShop.Data.SD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerShop.Controllers
{
    [Authorize(Roles = SD.Role_User_Admin)]
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.OrderDetails.Include(x => x.Order).Include(x => x.Product).ToList();
            return View(orders);
        }
    }
}
