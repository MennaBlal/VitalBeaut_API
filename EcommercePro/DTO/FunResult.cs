using EcommercePro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Any;

namespace EcommercePro.DTO
{
    public class FunResult
    {
        public int status { set; get; }
        public dynamic? data { set; get; }
        public dynamic? Errors { set; get; }
    }
}
