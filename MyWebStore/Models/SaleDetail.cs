namespace MyWebStore.Models
{
    public class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
