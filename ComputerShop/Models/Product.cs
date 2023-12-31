﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Server.IIS.Core;
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

        [Display(Name = "Producer")]
        public int ProducerId { get; set; }
		[ForeignKey("ProducerId")]
		[ValidateNever]
		public Producer Producer { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
        [Display(Name = "Cover image")]
		[NotMapped]
        public IFormFile? CoverImage { get; set; }
        public string? CoverImageUrl { get; set; }
        [Display(Name = "Product images")]
        [NotMapped]
        public IFormFileCollection? Images { get; set; }
		[ValidateNever]
		public List<ProductImage>? productImages { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }
	}
}
