using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    public class DeleteCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SQLEXPRESS"].ConnectionString;
        const string DeleteResourceBlobQuery = @"DELETE FROM Blobs WHERE Id IN (SELECT BlobId FROM CardsResources WHERE CardId = @Id)";
        const string DeleteCardResourceQuery = @"DELETE FROM CardsResources WHERE CardId = @Id";
        const string DeleteCardQuery = @"DELETE FROM Cards WHERE Id = @Id";

        public async Task ExecuteAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(DeleteResourceBlobQuery, new { Id = id }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardResourceQuery, new { Id = id }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardQuery, new { Id = id }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
