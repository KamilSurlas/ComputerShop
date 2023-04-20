using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.Models
{
    public class CategoryGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
       
    }
}
