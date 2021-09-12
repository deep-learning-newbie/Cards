using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    public class DeleteCardCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;
        
        const string DeleteCardRelationshipQuery = @"DELETE FROM CardsRelationship WHERE ParentId = @Id";
        const string DeleteImageCardResourceQuery = @"DELETE FROM ImageCardsResources WHERE ResourceId IN (SELECT CardId FROM CardsResources WHERE CardId = @Id)";
        const string DeleteTableCardResourceQuery = @"DELETE FROM TableCardsResources WHERE ResourceId IN (SELECT CardId FROM CardsResources WHERE CardId = @Id)";
        const string DeleteCardResourceQuery = @"DELETE FROM CardsResources WHERE CardId = @Id";
        const string DeleteCardQuery = @"DELETE FROM Cards WHERE Id = @Id";
        const string DeleteResourceBlobQuery = @"DELETE FROM Blobs WHERE Id IN (SELECT BlobId FROM ImageCardsResources WHERE ResourceId IN (SELECT Id FROM CardsResources WHERE CardId = @Id))";

        public async Task ExecuteAsync(int cardId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(DeleteCardRelationshipQuery, new { Id = cardId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteImageCardResourceQuery, new { Id = cardId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteTableCardResourceQuery, new { Id = cardId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardResourceQuery, new { Id = cardId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteCardQuery, new { Id = cardId }, transaction: transaction);
                    await connection.ExecuteAsync(DeleteResourceBlobQuery, new { Id = cardId }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
