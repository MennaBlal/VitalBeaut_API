using EcommercePro.DTO;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private IproductReview _productReview;
        public ProductReviewController(IproductReview productReview) { 
            _productReview = productReview;
        
        }
        [HttpGet]
        public ActionResult<List<ShowProductReview>>GetProuctReview(int productId)
        {
            return this._productReview.GetProductReview(productId);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult Add(ProductReviewDTO productReview)
        {
            if (ModelState.IsValid)
            {
                this._productReview.Add(productReview);
                return Ok();

            }
            return BadRequest("Review Not Added");
        }
 
    }
}
