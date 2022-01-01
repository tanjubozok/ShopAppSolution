using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.Business.Abstract
{
    public interface IOrderService
    {
        void Create(Order model);

        List<Order> GetOrders(string userId);
    }
}
