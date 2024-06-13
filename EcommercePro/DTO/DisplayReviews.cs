using EcommercePro.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePro.DTO
{
    public class DisplayReviews
    {
        public int Rating { get; set; }
        public string Comment { set; get; }
  
         public string username { set; get; }
        public string userimage { set; get; }

        public DateOnly CreatedDate { get; set; }
    }
}
