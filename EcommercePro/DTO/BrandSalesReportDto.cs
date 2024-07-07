
namespace EcommercePro.DTO
{
    public class BrandSalesReportDto
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalProfitBeforeAdmin { get; set; }
        public decimal TotalProfitAfterAdmin { get; set; }
        public int ProductsSold { get; set; }
        public List<ProductSalesDetailDTO> ProductSalesDetails { get; set; }
        public List<ProductSalesDetailDTO> TopSellingProducts { get; set; }
    }
}
