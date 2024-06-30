using EcommercePro.DTO;
using EcommercePro.Models;
using Org.BouncyCastle.Bcpg;

namespace EcommercePro.Repositiories
{
    public interface IwishList
    {
        public List<ShowWishList> WishListToUser(string userId);
        public void Add(WishListDTO wishList);
        public bool Delete( int favoriteId);

        public WishList IsExists(string userId, int productId);

    }
}
