using Microsoft.AspNetCore.Mvc;
using ComputerShop.Models;
using System.Diagnostics;
using ComputerShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
            //zmienic na shoppingcart -> dodac formularz w index
            ShoppingCart cart = new ShoppingCart()
            {
                Product = _context.Products.Include(x => x.Producer).Where(x => x.Id == productId)
                .Include(x => x.productImages).Where(x => x.Id == productId).FirstOrDefault(),
                ProductId = productId,
                Count = 1
            };
            return View(cart);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(ShoppingCart shoppingCart)
        {
            var cl = (ClaimsIdentity)User.Identity;
            var nameIdentifier = cl.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCart.ApplicationUserId = nameIdentifier.Value;

            ShoppingCart cart = await _context.ShoppingCarts.
                FirstOrDefaultAsync(x => x.ProductId == shoppingCart.ProductId && x.ApplicationUserId == nameIdentifier.Value);

            if (cart == null)
            {
                _context.ShoppingCarts.Add(shoppingCart);
            }
            else
            {
                cart.Count = shoppingCart?.Count + cart.Count ?? cart.Count;
                _context.ShoppingCarts.Update(cart);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
