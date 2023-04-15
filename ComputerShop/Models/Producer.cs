using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Models
{
	public class Producer
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public bool IsPromoted { get; set; }
	}
}
