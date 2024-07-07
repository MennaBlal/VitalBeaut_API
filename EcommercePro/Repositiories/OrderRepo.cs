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
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving order: {ex.Message}");
                throw;
            }
        }
        public async Task SaveOrderItemAsync(OrderItem orderitem)
        {
            try
            {
                orderitem.Id = 0;
                _context.OrderItems.Add(orderitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving order item: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<OrderDetailsDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.Payment)
                .Select(o => new OrderDetailsDto
                {
                    // Order specific properties
                    OrderId = o.Id,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
                    PaymentId = o.PaymentId,
                    PaymentFullName = o.Payment.FullName,
                    PaymentEmail = o.Payment.Email,
                    PaymentPhone = o.Payment.Phone,
                    PaymentCity = o.Payment.City,
                    PaymentState = o.Payment.State,
                    PaymentStreet = o.Payment.Street,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDetailsDto
                    {
                        Quantity = oi.Quantity,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        ProductImage = oi.Product.Images.FirstOrDefault().imagePath,  // Select first image
                        ProductPrice = oi.Product.Price,
                        ProductDescription = oi.Product.Description
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.Payment)
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsDto
                {
                    // Order specific properties
                    OrderId = o.Id,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
                    PaymentId = o.PaymentId,
                    PaymentFullName = o.Payment.FullName,
                    PaymentEmail = o.Payment.Email,
                    PaymentPhone = o.Payment.Phone,
                    PaymentCity = o.Payment.City,
                    PaymentState = o.Payment.State,
                    PaymentStreet = o.Payment.Street,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDetailsDto
                    {
                        Quantity = oi.Quantity,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        ProductImage = oi.Product.Images.FirstOrDefault().imagePath,  // Select first image
                        ProductPrice = oi.Product.Price,
                        ProductDescription = oi.Product.Description
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderDetailsDto>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.Payment)
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDetailsDto
                {
                    // Order specific properties
                    OrderId = o.Id,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
                    PaymentId = o.PaymentId,
                    PaymentFullName = o.Payment.FullName,
                    PaymentEmail = o.Payment.Email,
                    PaymentPhone = o.Payment.Phone,
                    PaymentCity = o.Payment.City,
                    PaymentState = o.Payment.State,
                    PaymentStreet = o.Payment.Street,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDetailsDto
                    {
                        Quantity = oi.Quantity,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        ProductImage = oi.Product.Images.FirstOrDefault().imagePath,  // Select first image
                        ProductPrice = oi.Product.Price,
                        ProductDescription = oi.Product.Description
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
