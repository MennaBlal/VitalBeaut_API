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
        private readonly IOrder _orderRepo;
        private readonly IProductRepository _productRepo;

        public PaymentController(IPaymentable paymentRepository, IOrder orderRepo, IProductRepository productRepo)
        {
            _paymentRepository = paymentRepository;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        [HttpPost]
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
            };

            var orderItems = paymentDto.OrderItems.Select(orderitem => new OrderItem
            {
                ProductId = orderitem.ProductId,
                Quantity = orderitem.Quantity,
                Price = orderitem.Price
            }).ToList();

            var order = new Order
            {
                Status = "Inprocessing",
                UserId = paymentDto.userId,
                Payment = payment,
                CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                OrderItems = orderItems
            };

            try
            {
                var charge = await _paymentRepository.ProcessPaymentAsync(payment, paymentDto.Amount);

                await _paymentRepository.SavePaymentAsync(payment);

                order.PaymentId = payment.Id;
                order.Status = "Completed";

                await _orderRepo.SaveOrderAsync(order);

                foreach (var orderItem in orderItems)
                {
                    orderItem.OrderId = order.Id;
                }

                foreach (var orderItem in orderItems)
                {
                    await _orderRepo.SaveOrderItemAsync(orderItem);
                }

                return Ok(charge);
            }
            catch (StripeException e)
            {
                order.Status = "Failed";
                await _orderRepo.SaveOrderAsync(order);

                Console.WriteLine($"StripeException: {e.StripeError.Message}");

                return BadRequest(new { error = e.StripeError.Message });
            }
            catch (Exception e)
            {
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
