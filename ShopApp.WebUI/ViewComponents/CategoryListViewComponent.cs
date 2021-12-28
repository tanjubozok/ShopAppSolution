using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var getCategoryList = _categoryService.GetAll();
            var model = new CategoryListModel
            {
                Categories = getCategoryList,
                SelectedCategory = RouteData.Values["category"]?.ToString()
            };
            return View(model);
        }
    }
}
