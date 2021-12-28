using ShopApp.Entities;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICategoryDal : IRepositoryDal<Category>
    {
        Category GetByIdWithProducts(int id);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
