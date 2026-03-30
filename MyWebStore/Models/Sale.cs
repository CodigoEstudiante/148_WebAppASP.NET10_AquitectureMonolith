namespace MyWebStore.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string CustomerName { get; set; }
        public DateOnly SaleDate { get; set; }
        public decimal TotalAmount { get; set; }

        public IEnumerable<SaleDetail> SaleDetails { get; set; }
    }
}
