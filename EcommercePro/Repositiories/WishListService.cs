using EcommercePro.DTO;
using EcommercePro.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Repositiories
{
    public class WishListService : IwishList
    {

        private readonly Context _context;

        public WishListService(Context context)  
        {
            _context = context;
        }
        public void Add(WishListDTO wishList)
        {
            this._context.WishList.Add(new WishList()
            {
                UserId = wishList.userId,
                productId = wishList.productId,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            });
            this._context.SaveChanges();
        }
        public WishList IsExists(string userId, int productId)
        {
            WishList ProductExists = this._context.WishList.FirstOrDefault(F => F.UserId == userId && F.productId == productId);
            return ProductExists;

        }


        public bool Delete(int wishId)
        {
            WishList wishList = this._context.WishList.FirstOrDefault(W=>W.Id ==  wishId);
            if (wishList != null) {

                this._context.WishList.Remove(wishList);
                this._context.SaveChanges();
                return true;

            }
            return false;
        }

        public List<ShowWishList> WishListToUser(string userId)
        {
            return this._context.WishList.Where(W=>W.UserId == userId)
                .Include(W=>W.product)
                .Select(W=>new ShowWishList()
                {
                 id=W.Id,
                 productName=W.product.Name,
                 productId=W.productId,
                 price=W.product.Price,
                 productImage=(W.product.Images.FirstOrDefault() != null)? W.product.Images.FirstOrDefault().imagePath:null,
                 //productImage=W.product.ImagePath



                }).ToList();
        }
    }
}
