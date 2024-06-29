using System.ComponentModel.DataAnnotations;

namespace EcommercePro.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name is Reqiured")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "The Email is Reqiured")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Phone is Reqiured")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "The City is Reqiured")]
        public string City { get; set; }
        [Required(ErrorMessage = "The State is Reqiured")]
        public string State { get; set; }
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "The Street is Reqiured")]
        public string Street { get; set; }
        //payment method
        public int CardNumber { set; get; }
        public int cvv { set; get; }
        public int Exp_Month { get; set; }
        public int Exp_Year { get; set; }
        public string StripeToken { get; set; }

    }
}
