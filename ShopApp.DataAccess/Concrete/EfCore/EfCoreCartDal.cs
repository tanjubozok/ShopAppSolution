using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
    }
}
