using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public double Price { get; set; }
		public int Amount { get; set; }


		public int ProducerId { get; set; }
		[ForeignKey("ProducerId")]
		[ValidateNever]
		public Producer Producer { get; set; }

		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
	}
}
