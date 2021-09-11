using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    // TODO: REMOVE FROM RELATIONSHIP FIRST
    public class DeleteCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SQLEXPRESS"].ConnectionString;
        const string QUERY = @"DELETE FROM Cards WHERE Id = @Id";

        public async Task ExecuteAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(QUERY, new { Id = id });
            }
        }
    }
}
