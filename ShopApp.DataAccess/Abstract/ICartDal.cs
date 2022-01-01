using ShopApp.Entities;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartDal : IRepositoryDal<Cart>
    {
        Cart GetByUserId(string userId);
        void DeleteFromCart(int cartId, int productId);
        void ClearCart(string cartId);
    }
}
