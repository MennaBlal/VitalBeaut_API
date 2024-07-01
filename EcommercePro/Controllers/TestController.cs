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
        public TestController(IFileService _FileService) {
            this.FileService = _FileService;
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
    }
}
