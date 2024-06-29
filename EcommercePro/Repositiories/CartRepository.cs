using EcommercePro.Models;
using EcommercePro.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Repositiories
{
    public class CartRepository : GenericRepo<Cart>, ICart
    {
        private readonly Context _context;
        public CartRepository(Context context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Cart> GetAllCartsWithProductDetails()
        {
            return _context.Set<Cart>()
                .Include(c => c.product)
            .ToList();
        }

        public Cart GetCartWithProductDetails(int id)
        {
            return _context.Set<Cart>()
                .Include(c => c.product)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}

