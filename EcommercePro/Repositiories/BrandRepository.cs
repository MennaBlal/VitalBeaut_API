using EcommercePro.DTO;
using EcommercePro.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Repositiories
{
    public class BrandRepository : IBrand
    {
        private Context _dbContext;

        public BrandRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Brand entity)
        {
            this._dbContext.Brands.Add(entity);
            Save();
        }

        public bool Delete(int id)
        {
            Brand entity = _dbContext.Brands.Find(id);
            if (entity != null)
            {
                _dbContext.Brands.Remove(entity);
                Save();
                return true;
            }
            return false;
        }

        public Brand Get(int id)
        {
            return _dbContext.Brands.Include(b=>b.User).FirstOrDefault(b=>b.Id==id);
        }
        public List<Brand> GetAll()
        {
            return _dbContext.Brands.Include(b=>b.User).ToList();
        }

        public bool Update(int id, Brand entity)
        {
            Brand existingEntity = _dbContext.Brands.Find(id);
            if (existingEntity != null)
            {
                existingEntity.Address = entity.Address;
                existingEntity.phonenumber2=entity.phonenumber2;
                existingEntity.TaxNumber = entity.TaxNumber;
                existingEntity.UserId = entity.UserId;
                existingEntity.commercialRegistrationImage = entity.commercialRegistrationImage;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public Brand getByUSersID(string USID)
        {
            Brand brand = _dbContext.Brands.FirstOrDefault(b => b.UserId == USID);
            return brand;
        }
       

    }
}
