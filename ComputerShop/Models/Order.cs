using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public double Price { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		[Display(Name = "Status")]
		public int StatusId { get; set; }
        [ForeignKey("StatusId")]
		public Status Status { get; set; }
	}
}
