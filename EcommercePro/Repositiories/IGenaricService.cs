
namespace EcommercePro.Repositiories
{
    public interface IGenaricService<T>
    {
        T Get(int id);
        bool Delete(int id);
        void Add(T entity);
        bool Update(int id, T entity);
        List<T> GetAll();
        void Save();
     }
}