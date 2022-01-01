using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.DataAccess.Abstract
{
    public interface IOrderDal : IRepositoryDal<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
