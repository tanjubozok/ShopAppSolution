using ShopApp.Entities;

namespace ShopApp.Business.Abstract
{
    public interface IOrderService
    {
        void Create(Order model);
    }
}
