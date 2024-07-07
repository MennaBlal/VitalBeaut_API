using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface ICart : IGenaricService<Cart>
    {
        IEnumerable<Cart> GetAllCartsWithProductDetails();
        Cart GetCartWithProductDetails(int id);
        bool DeleteCart(int id);
    }
    
}
