using EcommercePro.Models;
using Microsoft.OpenApi.Any;
using System.ComponentModel.DataAnnotations;

namespace EcommercePro.DTO
{
    public class ProductShow
    {
        public int Id { get; set; }
         public string Name { get; set; }
        public string Description { get; set; }
         public decimal Price { get; set; }
         public int Quentity { get; set; }
        public string image { get; set; }
         public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
    public class Result
    {
        public int? CurrentPage { set; get; }
        public int? totalPages { get; set; }
        public dynamic? data { get; set; }
        
    }
}
