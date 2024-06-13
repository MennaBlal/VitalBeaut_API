using EcommercePro.DTO;
using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IBrand:IGenaricService<Brand>
    {
        public Brand getByUSersID(string USID);
     }
}
