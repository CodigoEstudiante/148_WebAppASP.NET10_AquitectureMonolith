using MyWebStore.Models;
using System.Data;

namespace MyWebStore.Data
{
    public class SaleRepository(SqlConnectionFactory _connection)
    {

        private DataTable CreateDataTableDetail(IEnumerable<SaleDetail> detail)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Quantity", typeof(int));

            foreach(var item in detail) table.Rows.Add(item.ProductName,item.Price,item.Quantity);
            return table;

        }

        public void Create(Sale sale) {

            var detailsTable = CreateDataTableDetail(sale.SaleDetails);

            using var conn = _connection.CreateConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_createSale";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = sale.CustomerName;
            cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = sale.TotalAmount;
            cmd.Parameters.Add("@Details", SqlDbType.Structured, 100).Value = detailsTable;

            conn.Open();

            cmd.ExecuteNonQuery();

        }

        public void Edit(Sale sale)
        {

            var detailsTable = CreateDataTableDetail(sale.SaleDetails);

            using var conn = _connection.CreateConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_editSale";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = sale.SaleId;
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = sale.CustomerName;
            cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = sale.TotalAmount;
            cmd.Parameters.Add("@Details", SqlDbType.Structured, 100).Value = detailsTable;

            conn.Open();

            cmd.ExecuteNonQuery();

        }

        public void Delete(int SaleId)
        {


            using var conn = _connection.CreateConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_deleteSale";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = SaleId;

            conn.Open();

            cmd.ExecuteNonQuery();

        }

        public IEnumerable<Sale> GetAll()
        {


            using var conn = _connection.CreateConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_getSales";
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

           var reader =  cmd.ExecuteReader();

            var sales = new List<Sale>();

            while (reader.Read()) {
                sales.Add(new Sale()
                {
                    SaleId = Convert.ToInt32(reader["SaleId"]),
                       CustomerName = reader["CustomerName"].ToString(),
                    SaleDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["SaleDate"])),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                });
            }

            return sales;

        }

        public Sale Get(int SaleId)
        {
            using var conn = _connection.CreateConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_getSale";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = SaleId;

            conn.Open();

            var reader = cmd.ExecuteReader();

            var sale = new Sale();

            if (reader.Read())
            {
                sale = new Sale()
                {
                    SaleId = Convert.ToInt32(reader["SaleId"]),
                    CustomerName = reader["CustomerName"].ToString(),
                    SaleDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["SaleDate"])),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    SaleDetails = new List<SaleDetail>()
                };
            }

            if (reader.NextResult()) {
                var saleDetails = new List<SaleDetail>();

                while (reader.Read()) {
                    saleDetails.Add(new SaleDetail()
                    {
                        SaleDetailId = Convert.ToInt32(reader["SaleDetailId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SubTotal = Convert.ToDecimal(reader["SubTotal"])
                    });
                }

                sale.SaleDetails = saleDetails;
            }

            return sale;

        }
    }
}
