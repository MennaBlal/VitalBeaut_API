namespace EcommercePro.DTO
{
    public class PaymentDto
    {
        public string StripeToken { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string userId { get; set; }
        public int productId { get; set; }
        public int Quentity { get; set; }
        public int Amount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }


    }
}
