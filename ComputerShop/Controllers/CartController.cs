using ComputerShop.Data;
using ComputerShop.Data.SD;
using ComputerShop.Messages;
using ComputerShop.Models;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace ComputerShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ShoppingCartViewModel CartViewModel { get; set; }

        public CartController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartViewModel = new()
            {
                CartList = _context.ShoppingCarts.Where(x => x.ApplicationUserId == userId).Include(x => x.Product),
                Order = new()
            };

            IEnumerable<ProductImage> productImages = _context.ProductImages.ToList();

            foreach (var cart in CartViewModel.CartList)
            {
                cart.Product.productImages = productImages.Where(x => x.ProductId == cart.Product.Id).ToList();
                cart.Price = cart.Product.Price * cart.Count;
                CartViewModel.Order.Price += cart.Price;
            }

            return View(CartViewModel);
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartViewModel = new()
            {
                CartList = _context.ShoppingCarts.Where(x => x.ApplicationUserId == userId).Include(x => x.Product),
                Order = new()
            };

            CartViewModel.Order.AppUser = _context.Users.FirstOrDefault(x => x.Id == userId);

            CartViewModel.Order.Name = CartViewModel.Order.AppUser.Name;
            CartViewModel.Order.LastName = CartViewModel.Order.AppUser.LastName;
            CartViewModel.Order.PhoneNumber = CartViewModel.Order.AppUser.PhoneNumber;
            CartViewModel.Order.City = CartViewModel.Order.AppUser.City;
            CartViewModel.Order.StreetAddress = CartViewModel.Order.AppUser.StreetAddress;
            CartViewModel.Order.Region = CartViewModel.Order.AppUser.Region;
            CartViewModel.Order.PostalCode = CartViewModel.Order.AppUser.PostalCode;
            CartViewModel.Order.Country = CartViewModel.Order.AppUser.Country;

            foreach (var cart in CartViewModel.CartList)
            {
				cart.Price = cart.Product.Price * cart.Count;
				CartViewModel.Order.Price += cart.Price;
			}
            return View(CartViewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser appUser = _context.Users.FirstOrDefault(x => x.Id == userId);

            CartViewModel.CartList = _context.ShoppingCarts.Where(x => x.ApplicationUserId == userId)
                .Include(x => x.Product).ToList();

            CartViewModel.Order.OrderDate = DateTime.Now;
            CartViewModel.Order.ApplicationUserId = userId;
            CartViewModel.Order.Name = appUser.Name;
            CartViewModel.Order.LastName = appUser.LastName;
            CartViewModel.Order.Country = appUser.Country;
            CartViewModel.Order.City = appUser.City;

			foreach (var cart in CartViewModel.CartList)
			{
				cart.Price = cart.Product.Price * cart.Count;
				CartViewModel.Order.Price += cart.Price;
			}

            CartViewModel.Order.Status = _context.Statuses.FirstOrDefault(x => x.Name == StatusMessages.Shipping);

            _context.Orders.Add(CartViewModel.Order);
            _context.SaveChanges();

            foreach (var cart in CartViewModel.CartList)
            {
                OrderDetail orderDetail = new()
                {
                    OrderId = CartViewModel.Order.Id,
                    ProductId = cart.ProductId,
                    Quantity = cart.Count,
                    Price = cart.Price
                };
                _context.OrderDetails.Add(orderDetail);
				_context.SaveChanges();
			}

            var domain = "https://localhost:7239/";
			var options = new SessionCreateOptions()
			{
				SuccessUrl = domain + $"cart/OrderConfirmation?id={CartViewModel.Order.Id}",
				CancelUrl = domain + "cart/Index",
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in CartViewModel.CartList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
						Currency = "pln",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Name
						}
					},
					Quantity = item.Count
				};
				options.LineItems.Add(sessionLineItem);
			}

            var service = new SessionService();
            Session session = service.Create(options);
			Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
		}


		public IActionResult OrderConfirmation(int id)
		{
			Order header = _context.Orders.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

			List<ShoppingCart> shoppingCarts = _context.ShoppingCarts
				.Where(u => u.ApplicationUserId == header.ApplicationUserId).ToList();
			_context.ShoppingCarts.RemoveRange(shoppingCarts);
			_context.SaveChanges();
			return View(id);
		}


		public IActionResult Plus(int cartId)
        {
			var cartFromDb = _context.ShoppingCarts.FirstOrDefault(u => u.Id == cartId);
            cartFromDb.Count += 1;
			_context.ShoppingCarts.Update(cartFromDb);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Minus(int cartId)
		{
			var cartFromDb = _context.ShoppingCarts.FirstOrDefault(u => u.Id == cartId);

            if (cartFromDb.Count <= 1)
            {
				_context.ShoppingCarts.Remove(cartFromDb);
			}
            else
            {
                cartFromDb.Count -= 1;
			    _context.ShoppingCarts.Update(cartFromDb);
            }

			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int cartId)
		{
			var cart = _context.ShoppingCarts.FirstOrDefault(x => x.Id == cartId);
			_context.ShoppingCarts.Remove(cart);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
