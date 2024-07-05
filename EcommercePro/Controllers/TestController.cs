using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IFileService FileService;
        private IEmailService EmailService;
        public TestController(IFileService _FileService , IEmailService emailService)
        {
            this.FileService = _FileService;
            EmailService = emailService;
        }
        [HttpPost]
        public IActionResult uploadImage([FromForm] List<IFormFile> images)
        {
            try
            {
                foreach (var image in images)
                {
                    this.FileService.SaveImage(image);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("sendmail")]
        public IActionResult SendEmail(string Email, string body)
        {
            this.EmailService.SendEmail(Email ,body);
          
                return Ok();
            
        }
    }
}
