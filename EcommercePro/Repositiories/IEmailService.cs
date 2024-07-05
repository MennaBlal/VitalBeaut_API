namespace EcommercePro.Repositiories
{
    public interface IEmailService
    {
        public Task<string> SendEmail(string Email , string Meassage);
    }
}
