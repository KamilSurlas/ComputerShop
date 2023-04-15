using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Models
{
	public class Status
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
