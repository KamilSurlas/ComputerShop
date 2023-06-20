using ComputerShop.Data;
using ComputerShop.Models;
using Microsoft.AspNetCore.Identity;

namespace ComputerShop.DbConfig
{
	public class DbInitializer : IDbInitializer
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _db;

		public DbInitializer(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			ApplicationDbContext db)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_db = db;
		}
		public void Initialize()
		{
			if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
				_userManager.CreateAsync(new ApplicationUser()
				{
					UserName = "admin@polsl.pl",
					Email = "admin@polsl.pl",
					Name = "SklepAdmin",
					LastName = "SklepAdmin",
					City = "KatowicePolsl",
					StreetAddress = "Krasinskiego",
					PostalCode = "11-222",
					PhoneNumber = "111222333",
					Country = "Poland"

				}, "1qaz@WSX").GetAwaiter().GetResult();
				var role = _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
				var user = _userManager.FindByEmailAsync("admin@polsl.pl").GetAwaiter().GetResult();
				_userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
			}
		}
	}
}
