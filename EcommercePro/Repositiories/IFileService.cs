using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IFileService

    {
         
            public Tuple<int, string> SaveImage(IFormFile imageFile);
            public Task DeleteImage(string imageFileName);

            public void SaveImagesToDB(int productId ,string image );
            public void DeleteImagesFromDB(int productId);
            public List<ProductImages> GetAll(int productId);
            public void SaveChanges();




    }
}
