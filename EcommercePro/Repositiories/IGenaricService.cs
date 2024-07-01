
namespace EcommercePro.Repositiories
{
    public interface IGenaricService<T>
    {
        T Get(int id);
        void Add(T entity);
        bool Delete(int id);
        bool Update(int id, T entity);
        List<T> GetAll();
        void Save();
     }
}