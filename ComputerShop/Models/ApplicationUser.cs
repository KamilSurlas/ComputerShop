using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace ComputerShop.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string StreetAddress { get; set; }
		[Required]
		public string Region { get; set; }
		[Required]
		public string PostalCode { get; set; }
		[Required]
		public string Country { get; set; }

	}
}
