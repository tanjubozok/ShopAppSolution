using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Models;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 3;
            var getProductList = _productService.GetProductByCategory(category, page, pageSize);
            var model = new ProductListModel
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ITemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = getProductList,
            };
            return View(model);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var getProduct = _productService.GetProductDeteail((int)id);
            if (getProduct == null)
            {
                return NotFound();
            }
            var model = new ProductDetailModel
            {
                Product = getProduct,
                Categories = getProduct.ProductCategories.Select(x => x.Category).ToList()
            };
            return View(model);
        }
    }
}
