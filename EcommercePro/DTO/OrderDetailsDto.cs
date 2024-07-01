namespace EcommercePro.DTO
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string UserId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentFullName { get; set; }
        public string PaymentEmail { get; set; }
        public string PaymentPhone { get; set; }
        public string PaymentCity { get; set; }
        public string PaymentState { get; set; }
        public string PaymentStreet { get; set; }
        public DateOnly? CreatedDate { get; set; }
    }
}
