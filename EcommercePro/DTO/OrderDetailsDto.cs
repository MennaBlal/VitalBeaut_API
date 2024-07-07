namespace EcommercePro.DTO
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public int PaymentId { get; set; }
        public string PaymentFullName { get; set; }
        public string PaymentEmail { get; set; }
        public string PaymentPhone { get; set; }
        public string PaymentCity { get; set; }
        public string PaymentState { get; set; }
        public string PaymentStreet { get; set; }
        public List<OrderItemDetailsDto> OrderItems { get; set; }
    }
}
