using Microsoft.Data.SqlClient;

namespace MyWebStore.Data
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
