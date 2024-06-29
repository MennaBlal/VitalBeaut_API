using EcommercePro.DTO;
using EcommercePro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace EcommercePro.Repositiories
{
    public class OrderRepo : IOrder
    {
        private readonly Context _context;
        public OrderRepo(Context context)
        {
            _context = context;
        }
        public async Task SaveOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<OrderDetailsDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.product)
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Select(o => new OrderDetailsDto
                {
                    Id = o.Id,
                    Quantity = o.Quentity,
                    Status = o.Status,
                    ProductId = o.productId,
                    ProductName = o.product.Name,
                    ProductPrice = o.product.Price,
                    ProductDescription = o.product.Description,
                    UserId = o.UserId,
                    PaymentId = o.PaymentId,
                    PaymentFullName = o.Payment.FullName,
                    PaymentEmail = o.Payment.Email,
                    PaymentPhone = o.Payment.Phone,
                    PaymentCity = o.Payment.City,
                    PaymentState = o.Payment.State,
                    PaymentStreet = o.Payment.Street,
                    CreatedDate = o.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.product)
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsDto
                {
                    Id = o.Id,
                    Quantity = o.Quentity,
                    Status = o.Status,
                    ProductId = o.productId,
                    ProductName = o.product.Name,
                    ProductPrice = o.product.Price,
                    ProductDescription = o.product.Description,
                    UserId = o.UserId,
                    PaymentId = o.PaymentId,
                    PaymentFullName = o.Payment.FullName,
                    PaymentEmail = o.Payment.Email,
                    PaymentPhone = o.Payment.Phone,
                    PaymentCity = o.Payment.City,
                    PaymentState = o.Payment.State,
                    PaymentStreet = o.Payment.Street,
                    CreatedDate = o.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderDetailsDto>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.product)
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDetailsDto
                {
                    Id = o.Id,
                    Quantity = o.Quentity,
                    Status = o.Status,
                    ProductId = o.productId,
                    ProductName = o.product.Name,
                    ProductPrice = o.product.Price,
                    ProductDescription = o.product.Description,
                    UserId = o.UserId,
                    PaymentId = o.PaymentId,
                    PaymentFullName = o.Payment.FullName,
                    PaymentEmail = o.Payment.Email,
                    PaymentPhone = o.Payment.Phone,
                    PaymentCity = o.Payment.City,
                    PaymentState = o.Payment.State,
                    PaymentStreet = o.Payment.Street,
                    CreatedDate = o.CreatedDate
                })
                .ToListAsync();
        }
    }
}
