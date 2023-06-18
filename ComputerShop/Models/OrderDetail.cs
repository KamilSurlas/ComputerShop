using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.Models
{
	public class OrderDetail
	{
		[Key]
		public int Id { get; set; }
		public int Quantity { get; set; }

        [Display(Name = "Order")]
        public int OrderId { get; set; }
		[ForeignKey("OrderId")]
		[ValidateNever]
		public Order Order { get; set; }
		public double Price { get; set; }
        [Display(Name = "Product")]
        public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }
	}
}
