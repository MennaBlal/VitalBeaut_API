using EcommercePro.Models;
using Stripe;

namespace EcommercePro.Repositiories
{
    public interface IPaymentable
    {
        Task<Charge> ProcessPaymentAsync(Payment payment, int amount);
         Task SavePaymentAsync(Payment payment);

    }
}
