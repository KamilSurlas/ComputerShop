using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
        public int CategoryGroupId { get; set; }
        [ForeignKey("CategoryGroupId")]
        [ValidateNever]
        public CategoryGroup CategoryGroup { get; set; }
    }
}
