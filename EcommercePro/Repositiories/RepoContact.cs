using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public class RepoContact:IContact
    {
        private readonly Context _dbContext;


        public RepoContact(Context context)
        {
            _dbContext = context;
        }
        public List<Contact> GetAll()
        {
            return _dbContext.Set<Contact>().ToList();
        }

        public void Insert(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            _dbContext.Add(contact);

        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

    }



}