namespace EcommercePro.DTO
{
    public class CartData
    {
        public int Id { get; set; }
        public int Quentity { get; set; }
        public int productId { set; get; }
        public string UserId { set; get; }
        public DateOnly? CreatedDate { get; set; }

        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
