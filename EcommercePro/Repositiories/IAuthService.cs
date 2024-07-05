using EcommercePro.DTO;

namespace EcommercePro.Repositiories
{
    public interface IAuthService
    {
        public  Task<FunResult> CreateAcount(UserRegister user);
        public FunResult AddAsBrand(UserRegister userData, string userId);
        public Task<FunResult> AddAsUser(UserRegister userData);
        public  Task<string> ConfirmEmail(string userId, string code);


    }
}
