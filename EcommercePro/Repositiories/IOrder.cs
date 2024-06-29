using EcommercePro.DTO;
using EcommercePro.Models;

namespace EcommercePro.Repositiories
{
    public interface IOrder
    {

        Task SaveOrderAsync(Order order);
        Task<IEnumerable<OrderDetailsDto>> GetAllOrdersAsync();
        Task<OrderDetailsDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDetailsDto>> GetOrdersByUserIdAsync(string userId);

    }
}
