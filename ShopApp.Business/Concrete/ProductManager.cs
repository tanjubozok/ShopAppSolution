using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public void Create(Product entity)
        {
            _productDal.Create(entity);
        }

        public void Delete(Product entity)
        {
            _productDal.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productDal.GetAll().ToList();
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productDal.GetCountByCategory(category);
        }

        public List<Product> GetProductByCategory(string category, int page, int pageSize)
        {
            return _productDal.GetProductByCategory(category, page, pageSize);
        }

        public Product GetProductDeteail(int id)
        {
            return _productDal.GetProductDetail(id);
        }

        public void Update(Product entity)
        {
            _productDal.Update(entity);
        }
    }
}
