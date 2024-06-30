using EcommercePro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Any;

namespace EcommercePro.DTO
{
    public class FunResult
    {
        public int status { set; get; }
        public ApplicationUser data { set; get; }
        public List<IdentityError> Errors { set; get; }
    }
}
