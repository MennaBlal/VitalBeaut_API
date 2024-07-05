using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentable _paymentRepository;
        private readonly IOrder orderRepo;
        private readonly IProductRepository productRepo;
        public PaymentController(IPaymentable paymentRepository , IOrder order , IProductRepository product)
        {
            _paymentRepository = paymentRepository;
            orderRepo = order;
            productRepo = product;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] PaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = new Payment
            {
                FullName = paymentDto.FullName,
                Email = paymentDto.Email,
                Phone = paymentDto.Phone,
                City = paymentDto.City,
                State = paymentDto.State,
                ZipCode = paymentDto.ZipCode,
                Street = paymentDto.Street,
                StripeToken = paymentDto.StripeToken
                // Add other payment details as needed
            };

            var order = new Order
            {
                Quentity = paymentDto.Quentity, // Assuming you have Quantity in paymentDto
                Status = "Inprocessing", // Set initial status
                productId = paymentDto.productId, // Assuming you have ProductId in paymentDto
                UserId = paymentDto.userId, // Assuming you have UserId in paymentDto
                Payment = payment, // Associate the payment with the order
                CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow) // Set the current date
            };

            try
            {
                var charge = await _paymentRepository.ProcessPaymentAsync(payment, paymentDto.Amount);

                // Save payment to database
                await _paymentRepository.SavePaymentAsync(payment);

                // Update the order with the payment ID and status
                order.PaymentId = payment.Id;
                order.Status = "Completed";

                // Save order to database
                await orderRepo.SaveOrderAsync(order);

                return Ok(charge);
            }
            catch (StripeException e)
            {
                // Update order status to failed
                order.Status = "Failed";
                await orderRepo.SaveOrderAsync(order); // Save the failed order to the database

                // Log the Stripe exception
                Console.WriteLine($"StripeException: {e.StripeError.Message}");

                return BadRequest(new { error = e.StripeError.Message });
            }
            catch (Exception e)
            {
                // Log the exception details
                Console.WriteLine($"Exception: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {e.InnerException.Message}");
                }

                return StatusCode(500, new { error = e.Message });
            }
        }
    }
    
}
