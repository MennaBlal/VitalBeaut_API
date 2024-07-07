using EcommercePro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace EcommercePro.Repositiories
{
    public class PaymentRepo : IPaymentable
    {
        private readonly StripeSettings _stripeSettings;
        private readonly Context _context;

        public PaymentRepo(IOptions<StripeSettings> stripeSettings, Context context)
        {
            _stripeSettings = stripeSettings.Value;
            _context = context;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }
        public async Task<Charge> ProcessPaymentAsync(Payment payment, int amount)
        {
            var options = new ChargeCreateOptions
            {
                Amount = amount, // Amount in cents (e.g., $50.00)
                Currency = "usd",
                Description = "Sample Charge",
                Source = payment.StripeToken // Use the token created from the client-side
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            return charge;
        }

        // Add method to save payment to the database
        public async Task SavePaymentAsync(Payment payment)
        {
            try
            {
                _context.payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Error saving payment: {ex.Message}");
                throw; // Re-throw the exception to propagate it upwards
            }
        }

    }
}

