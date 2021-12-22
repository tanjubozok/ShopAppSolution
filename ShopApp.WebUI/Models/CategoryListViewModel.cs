using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
    public class CategoryListViewModel
    {
        public List<Category> Categories { get; set; }
        public string SelectedCategory { get; set; }
    }
}
