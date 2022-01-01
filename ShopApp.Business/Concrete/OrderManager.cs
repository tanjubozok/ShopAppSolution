using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System.Collections.Generic;

namespace ShopApp.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public void Create(Order model)
        {
            _orderDal.Create(model);
        }

        public List<Order> GetOrders(string userId)
        {
            return _orderDal.GetOrders(userId);
        }
    }
}
