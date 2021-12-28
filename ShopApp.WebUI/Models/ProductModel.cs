﻿using ShopApp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Ürün isimi minimum 10 karakter, maximum 60 karakter olmalıdır.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Ürün açıklaması minimum 20 karakter, maximum 100 karakter olmalıdır.")]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Fiyat belirtiniz")]
        [Range(1, 10000)]
        public decimal? Price { get; set; }

        public List<Category> SelectCategories { get; set; }
    }
}
