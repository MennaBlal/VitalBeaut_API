using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private IwishList _wishListService;
        public WishListController(IwishList wishListService) { 
        
            this._wishListService = wishListService;
        }
        [HttpGet]
        public ActionResult<List<ShowWishList>>GetUserWishList(string userid)
        {
           return this._wishListService.WishListToUser(userid);
        }
        [HttpPost]
        public IActionResult Add(WishListDTO wishList)
        {
            if (ModelState.IsValid)
            {
                WishList IsExists = this._wishListService.IsExists(wishList.userId, wishList.productId);
                if (IsExists != null)
                {
                    return BadRequest("Product Is Exists");
                }

                this._wishListService.Add(wishList);
                return Ok();
            }
            return BadRequest(ModelState);
            
        }
        [HttpDelete]
        public IActionResult Delete(int favoriteId)
        {
           
           bool isDeleted = this._wishListService.Delete(favoriteId);
            if (isDeleted)
            {
                return Ok();

            }
            return BadRequest("Faild To Delete the product from WishList");
        }


    }
}
