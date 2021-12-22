using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductDal : IRepositoryDal<Product>
    {
        List<Product> GetProductByCategory(string category, int page, int pageSize);
        Product GetProductDetail(int id);
        int GetCountByCategory(string category);
    }
}
