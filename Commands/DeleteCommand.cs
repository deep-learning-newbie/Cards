using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    // TODO: REMOVE FROM RELATIONSHIP FIRST
    public class DeleteCommand
    {
        string _connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CardsDB;Integrated Security=True;";
        const string QUERY = @"DELETE FROM Cards WHERE Id = @Id";

        public async Task ExecuteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(QUERY, new { Id = id });
            }
        }
    }
}
