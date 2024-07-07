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
            var brands = await _context.Brands.Include(b => b.User)
       .ToListAsync();

            var report = new List<AdminSalesReportDto>();
            decimal totalAdminProfit = 0;

            foreach (var brand in brands)
            {
                var totalSales = await _context.OrderItems
                    .Where(oi => oi.Product.BrandId == brand.Id && oi.Order.Status == "completed" &&
                                 oi.Order.CreatedDate.HasValue && oi.Order.CreatedDate.Value.Month == month &&
                                 oi.Order.CreatedDate.Value.Year == year)
                    .SumAsync(oi => oi.Quantity * oi.Price);

                var adminProfit = totalSales * 0.15m;
                var totalBrandProfit = totalSales * 0.85m;
                totalAdminProfit += adminProfit;

                report.Add(new AdminSalesReportDto
                {
                    BrandName = brand.User?.UserName ?? "Unknown",
                    SalesAmount = totalSales,
                    AdminProfit = adminProfit,
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

            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.Product.BrandId == brandId && oi.Order.Status == "completed" && oi.Order.CreatedDate.HasValue && oi.Order.CreatedDate.Value.Month == month && oi.Order.CreatedDate.Value.Year == year)
                .ToListAsync();

            var report = new BrandSalesReportDto
            {
                Month = new DateTime(year, month, 1).ToString("MMMM"),
                Year = year,
                ProductSalesDetails = new List<ProductSalesDetailDTO>()
            };

            decimal totalSales = 0;
            int totalProductsSold = 0;

            foreach (var productGroup in orderItems.GroupBy(oi => oi.Product))
            {
                var productSales = productGroup.Sum(oi => oi.Quantity * oi.Price);
                var productQuantitySold = productGroup.Sum(oi => oi.Quantity);

                totalSales += productSales;
                totalProductsSold += productQuantitySold;

                if (productSales > 0)
                {
                    report.ProductSalesDetails.Add(new ProductSalesDetailDTO
                    {
                        ProductName = productGroup.Key.Name,
                        QuantitySold = productQuantitySold,
                        TotalSales = productSales,
                        ProfitPercentage = 0
                    });
                }
            }
            foreach (var productDetail in report.ProductSalesDetails)
            {
                productDetail.ProfitPercentage = (productDetail.TotalSales / totalSales) * 100;
            }
            report.TotalSales = totalSales;
            report.TotalProfitBeforeAdmin = totalSales;
            report.TotalProfitAfterAdmin = totalSales * 0.85m;
            report.ProductsSold = totalProductsSold;
            report.TopSellingProducts = report.ProductSalesDetails
                .OrderByDescending(p => p.ProfitPercentage)
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
