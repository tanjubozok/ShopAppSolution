using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);

        void Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
    }
}
