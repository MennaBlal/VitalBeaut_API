using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteReviewController : ControllerBase
    {
        WebsiteReviewRepo WebsiteReviewRepo;
        IWebsiteReview websiteReview;

        public WebsiteReviewController(IWebsiteReview WebsiteReview1)
        {
            websiteReview = WebsiteReview1;
        }

        [HttpGet]
        public ActionResult<List<DisplayReviews>> GetAll()
        {
            List<WebsiteReview> reviewList = websiteReview.GetAll();

            List<DisplayReviews> reviews = reviewList.Select(review => new DisplayReviews()
            {
                Comment=review.Comment,
                userimage=review.User.Image,
                username=review.User.UserName,
                CreatedDate=(DateOnly)review.CreatedDate,
                Rating=review.Rating


            }).ToList();

            return reviews;

        }

        [HttpPost]
        public IActionResult Add(WebsiteDTO newReview)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    websiteReview.Insert(new WebsiteReview()
                    {
                        Rating = newReview.Rating,
                        Comment = newReview.Comment,
                        UserId = newReview.UserId,
                        CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                    });
                    websiteReview.Save();

                    return Ok("Review added successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while adding the review. Please try again later.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}