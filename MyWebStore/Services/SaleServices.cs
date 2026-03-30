using MyWebStore.Data;
using MyWebStore.DTOs;
using MyWebStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MyWebStore.Services
{
    public class SaleServices(SaleRepository _repo)
    {
        public void Create(SaleDTO sale)
        {

            if (string.IsNullOrEmpty(sale.CustomerName))
                throw new ValidationException("Customer Name is required");

            var model = new Sale()
            {
                CustomerName = sale.CustomerName,
                TotalAmount = sale.TotalAmount,
                SaleDetails = sale.SaleDetails.Select(item => new SaleDetail
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                })
            };

            _repo.Create(model);
        }

        public void Edit(SaleDTO sale)
        {

            if (string.IsNullOrEmpty(sale.CustomerName))
                throw new ValidationException("Customer Name is required");

            var model = new Sale()
            {
                SaleId = sale.SaleId,
                CustomerName = sale.CustomerName,
                TotalAmount = sale.TotalAmount,
                SaleDetails = sale.SaleDetails.Select(item => new SaleDetail
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                })
            };

            _repo.Edit(model);
        }

        public void Delete(int SaleId)
        {

            if (SaleId == 0)
                throw new ValidationException("Sale Id is required");

            _repo.Delete(SaleId);
        }

        public IEnumerable<SaleDTO> GetAll()
        {
            var sales = _repo.GetAll();

            IEnumerable<SaleDTO> salesDto = sales.Select(item => new SaleDTO
            {
                SaleId = item.SaleId,
                CustomerName = item.CustomerName,
                SaleDate = item.SaleDate,
                TotalAmount = item.TotalAmount
            });

            return salesDto;
        }

        public SaleDTO Get(int SaleId)
        {
            if (SaleId == 0)
                throw new ValidationException("Sale Id is required");

            var model = _repo.Get(SaleId);


            var saleDto = new SaleDTO
            {
                SaleId = model.SaleId,
                CustomerName = model.CustomerName,
                SaleDate = model.SaleDate,
                TotalAmount = model.TotalAmount,
                SaleDetails = model.SaleDetails.Select(item => new SaleDetailDTO
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                })
            };

            return saleDto;
        }

    }
}
