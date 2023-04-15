using ComputerShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerShop.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{}

		public DbSet<Product> Products { get; set; }
		public DbSet<Producer> Producers { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Status> Statuses { get; set; }

	}
}
