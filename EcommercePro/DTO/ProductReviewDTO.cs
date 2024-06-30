namespace EcommercePro.DTO
{
    public class ProductReviewDTO
    {
        public int Rating { get; set; }
        public string? Comment { set; get; }
        public string UserId { set; get; }
        public int ProductId { set; get; }

    }
    public class ShowProductReview
    {
        public int Rating { get; set; }
        public string? Comment { set; get; }
        public string username { set; get; }
        public string userimage { set; get; }
        public string  date { set; get; }
     }

}
