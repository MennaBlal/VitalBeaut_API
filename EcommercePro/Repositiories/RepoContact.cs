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

        public bool Delete(int contactId)
        {
           
                Contact contact = this._dbContext.Contacts.FirstOrDefault(contact => contact.Id == contactId);
            if (contact != null)
            {
                this._dbContext.Contacts.Remove(contact);
                this.Save();
                return true;
            }
            return false;

              
        }

        public List<Contact> GetAll()
        {
            return _dbContext.Set<Contact>().ToList();
        }

        public void Insert(Contact contact)
        {
            if (contact != null)
            {
                _dbContext.Add(contact);

            }


        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

    }



}