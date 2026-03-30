using MyWebStore.Models;

namespace MyWebStore.DTOs
{
    public class SaleDTO
    {
        public int SaleId { get; set; }
        public string CustomerName { get; set; }
        public DateOnly SaleDate { get; set; }
        public decimal TotalAmount { get; set; }

        public IEnumerable<SaleDetailDTO> SaleDetails { get; set; }
    }
}
