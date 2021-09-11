using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    public class DeleteCardResourceCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;
        
        const string DeleteResourceBlobQuery = @"DELETE FROM Blobs WHERE Id IN (SELECT BlobId FROM CardsResources WHERE Id = @Id)";
        const string DeleteCardRelationshipQuery = @"DELETE FROM CardsRelationship WHERE ChildId = @Id";
        const string DeleteCardResourceQuery = @"DELETE FROM CardsResources WHERE Id = @Id";

        public async Task ExecuteAsync(int resourceId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(DeleteResourceBlobQuery, new { Id = resourceId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardRelationshipQuery, new { Id = resourceId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardResourceQuery, new { Id = resourceId }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
