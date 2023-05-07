using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ComputerShop.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public string URL { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public int ProductId { get; set; }
    }
}
