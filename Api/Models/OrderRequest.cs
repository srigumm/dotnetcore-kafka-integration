namespace Api.Models
{
    public class OrderRequest
    {
        public int id { get; set; }
        public string productname { get; set; }
        public int quantity { get; set; }

        public OrderStatus status{get;set;}
    }
    public enum OrderStatus{
        IN_PROGRESS,
        COMPLETED,
        REJECTED
    }
}