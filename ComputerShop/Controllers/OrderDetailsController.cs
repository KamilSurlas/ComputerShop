using ComputerShop.Data;
using ComputerShop.Data.SD;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var orders = _context.OrderDetails.Include(x => x.Order).ThenInclude(x => x.Status).Include(x => x.Product).ToList();
            return View(orders);
        }

        public IActionResult EditOrderStatus(int? orderId)
        {
            if (orderId == null || orderId == 0)
            {
                return NotFound();
            }

            var orderViewModel = new OrderViewModel()
            {
                Order = new(),
                StatusesList = _context.Statuses.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
                return NotFound();

            orderViewModel.Order = order;

            return View(orderViewModel);
        }

        [HttpPost]
        public IActionResult Edit(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Update(orderViewModel.Order);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
