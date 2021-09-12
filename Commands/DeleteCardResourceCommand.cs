using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    public class DeleteCardResourceCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;

        const string DeleteImageCardResourceQuery = @"DELETE FROM ImageCardsResources WHERE ResourceId = @Id";
        const string DeleteTableCardResourceQuery = @"DELETE FROM TableCardsResources WHERE ResourceId = @Id";
        const string DeleteCardResourceQuery = @"DELETE FROM CardsResources WHERE Id = @Id";
        const string DeleteResourceBlobQuery = @"DELETE FROM Blobs WHERE Id IN (SELECT BlobId FROM ImageCardsResources WHERE ResourceId = @Id)";

        public async Task ExecuteAsync(int resourceId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(DeleteImageCardResourceQuery, new { Id = resourceId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteTableCardResourceQuery, new { Id = resourceId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardResourceQuery, new { Id = resourceId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteResourceBlobQuery, new { Id = resourceId }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
