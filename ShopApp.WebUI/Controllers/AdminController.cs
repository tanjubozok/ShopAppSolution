﻿using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult ProductList()
        {
            var getProductList = _productService.GetAll();
            var model = new ProductListModel
            {
                Products = getProductList
            };
            return View(model);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product
            {
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Name = model.Name,
                Price = model.Price
            };
            _productService.Create(entity);
            return RedirectToAction("Index");
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _productService.GetByIdWithCategories((int)id);
            var getProductCategoryList = entity.ProductCategories.Select(x => x.Category).ToList();
            if (entity == null)
            {
                return NotFound();
            }
            var model = new ProductModel()
            {
                Description = entity.Description,
                Id = entity.Id,
                ImageUrl = entity.ImageUrl,
                Name = entity.Name,
                Price = entity.Price,
                SelectCategories = getProductCategoryList
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel model, int[] categoryIds)
        {
            var entity = _productService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;
            entity.Name = model.Name;
            entity.Price = model.Price;
            _productService.Update(entity, categoryIds);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            return RedirectToAction("Index");
        }

        public IActionResult CategoryList()
        {
            var getCategoryList = _categoryService.GetAll();
            var model = new CategoryListModel
            {
                Categories = getCategoryList
            };
            return View(model);
        }

        public IActionResult EditCategory(int Id)
        {
            var entity = _categoryService.GetByIdWithProducts(Id);
            var getProductCategoryList = entity.ProductCategories.Select(p => p.Product).ToList();
            var model = new CategoryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Products = getProductCategoryList
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model, int categoryId)
        {
            var entity = _categoryService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            _categoryService.Update(entity);
            return RedirectToAction("CategoryList");
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category
            {
                Name = model.Name
            };
            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return Redirect("/admin/editcategory/" + categoryId);
        }
    }
}
