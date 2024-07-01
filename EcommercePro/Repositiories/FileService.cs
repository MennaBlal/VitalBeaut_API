using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Hosting;
 
namespace ProductMiniApi.Repository.Implementation
{
    public class FileService : IFileService
    {
        private IWebHostEnvironment environment;
        private Context _dbContext; 
        public FileService(IWebHostEnvironment env , Context dbContext)
        {
            this.environment = env;
            this._dbContext = dbContext;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var contentPath = this.environment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public async Task DeleteImage(string imageFileName)
        {
            var contentPath = this.environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads", imageFileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void SaveImagesToDB(int productId, string image)
        {
            this._dbContext.ProductImages.Add(new ProductImages()
            {
                productId = productId,
                imagePath = image 
            });
        }

        public void DeleteImagesFromDB(int productId)
        {
            ProductImages image = _dbContext.ProductImages.FirstOrDefault(Image=>Image.productId ==  productId);
           if(image != null)
            {
                this._dbContext.ProductImages.Remove(image);
            }
           
        }
        public void SaveChanges()
        {
            this._dbContext.SaveChanges();
        }
        public List<ProductImages> GetAll(int productId)
        {
            return this._dbContext.ProductImages.Where(image => image.productId == productId).ToList();
        }

    }
}
