using EcommercePro.DTO;
using EcommercePro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly Context _context;
        private UserManager<ApplicationUser> _userManager;

        public ReportController(Context context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("admin-sales-report/{month}/{year}")]
        public async Task<ActionResult<List<AdminSalesReportDto>>> GetAdminSalesReport(int month, int year)
        {
            var brands = await _context.Brands.Include(b => b.User).ToListAsync();

            var report = new List<AdminSalesReportDto>();
            decimal totalAdminProfit = 0;

            foreach (var brand in brands)
            {
                var brandOrders = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Product.BrandId == brand.Id &&
                                 oi.Order.Status == "completed" &&
                                 oi.Order.CreatedDate.HasValue &&
                                 oi.Order.CreatedDate.Value.Month == month &&
                                 oi.Order.CreatedDate.Value.Year == year)
                    .ToListAsync();

                decimal brandTotalSales = 0;
                decimal brandAdminProfit = 0;

                foreach (var orderItem in brandOrders)
                {
                    decimal productTotalSales = orderItem.Quantity * (orderItem.Price * 0.85m); // Reduce price by 15%
                    decimal productAdminProfit = orderItem.Quantity * (orderItem.Price * 0.15m); // Admin profit is 15% of original price

                    brandTotalSales += productTotalSales;
                    brandAdminProfit += productAdminProfit;
                }

                var totalBrandProfit = brandTotalSales - brandAdminProfit;
                totalAdminProfit += brandAdminProfit;

                report.Add(new AdminSalesReportDto
                {
                    BrandName = brand.User?.UserName ?? "Unknown",
                    SalesAmount = brandTotalSales,
                    AdminProfit = brandAdminProfit,
                    TotalBrandProfit = totalBrandProfit,
                });
            }

            var response = new
            {
                Report = report,
                TotalAdminProfit = totalAdminProfit,
            };

            return Ok(response);
        }

        [HttpGet("brand-sales-report/{brandId}/{month}/{year}")]
        public async Task<ActionResult<BrandSalesReportDto>> GetBrandSalesReport(int brandId, int month, int year)
        {
            var brand = await _context.Brands
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == brandId);

            if (brand == null)
            {
                return NotFound();
            }

            var brandOrders = await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Product.BrandId == brandId &&
                             oi.Order.Status == "completed" &&
                             oi.Order.CreatedDate.HasValue &&
                             oi.Order.CreatedDate.Value.Month == month &&
                             oi.Order.CreatedDate.Value.Year == year)
                .ToListAsync();

            var userCount = await _context.Orders
      .Where(o => o.OrderItems.Any(oi => oi.Product.BrandId == brandId) &&
                  o.Status == "completed" &&
                  o.CreatedDate.HasValue &&
                  o.CreatedDate.Value.Month == month &&
                  o.CreatedDate.Value.Year == year)
      .Select(o => o.UserId)
      .Distinct()
      .CountAsync();

            var report = new BrandSalesReportDto
            {
                Month = new DateTime(year, month, 1).ToString("MMMM"),
                Year = year,
                UserCount = userCount, // Include user count in the report
                ProductSalesDetails = new List<ProductSalesDetailDTO>()
            };

            decimal brandTotalSales = 0;
            decimal brandAdminProfit = 0;

            foreach (var orderItem in brandOrders)
            {
                decimal productTotalSales = orderItem.Quantity * (orderItem.Price * 0.85m); // Reduce price by 15%
                decimal productAdminProfit = orderItem.Quantity * (orderItem.Price * 0.15m); // Admin profit is 15% of original price

                brandTotalSales += productTotalSales;
                brandAdminProfit += productAdminProfit;

                if (productTotalSales > 0)
                {
                    report.ProductSalesDetails.Add(new ProductSalesDetailDTO
                    {
                        ProductName = orderItem.Product.Name,
                        QuantitySold = orderItem.Quantity,
                        TotalSales = productTotalSales,
                        ProfitPercentage = (productTotalSales / brandTotalSales) * 100
                    });
                }
            }

            report.TotalSales = brandTotalSales;
            report.TotalProfitBeforeAdmin = brandTotalSales;
            report.TotalProfitAfterAdmin = brandTotalSales - brandAdminProfit;
            report.ProductsSold = brandOrders.Sum(oi => oi.Quantity);
            report.UserCount = userCount;
            report.TopSellingProducts = report.ProductSalesDetails
                .OrderByDescending(p => p.QuantitySold)
                .Take(2)
                .ToList();

            return Ok(report);
        }

        [HttpGet("genaral-calculation")]
        public IActionResult Result()
        {
            int brands = this._context.Brands.Where(b => b.User.IsDisable == false && b.Status == "Accepted").Count();
            int users = this._userManager.Users.Where(u=>u.IsDisable==false).Count();
            int products = this._context.Products.Where(p=>p.IsDeleted == false).Count();
            return Ok(
                new {
                    brands = brands,
                    users = users ,
                    products=products
                }
                );
        }
        [HttpGet("genaral-calculation2")]
        public IActionResult Result2(int brandId)
        {
            //int brands = this._context.Brands.Count();
            //int users = this._userManager.Users.Count();
            int products = this._context.Products.Where(p=>p.BrandId == brandId && p.IsDeleted == false).Count();
            return Ok(
                new {products = products }
                );
        }
    }


}
