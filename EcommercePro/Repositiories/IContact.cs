using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IContact
    {
        public List<Contact>GetAll();
        void Insert(Contact cotact);
        void Save();

    }
}
