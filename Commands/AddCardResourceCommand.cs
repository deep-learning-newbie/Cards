using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Models;

namespace Commands
{
    public class AddCardResourceCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;

        const string InsertCardResourceQuery = @"INSERT INTO CardsResources (CardId, ResourceType) VALUES (@CardId, @ResourceType)";
        const string GetLastCardResourceIdQuery = @"SELECT MAX(Id) FROM CardsResources";
        const string GetLastBlobIdQuery = @"SELECT MAX(Id) FROM Blobs"; // todo: make upload in future
        const string InsertImageCardResourceQuery = @"INSERT INTO ImageCardsResources (BlobId, Body, ResourceId) VALUES (@BlobId, @Body, @ResourceId)";
        const string InsertTableCardResourceQuery = @"INSERT INTO TableCardsResources (Column1, Column2, ResourceId) VALUES (@Column1, @Column2, @ResourceId)";

        public async Task ExecuteAsync(int cardId, ResourceType resourceType, string column1, string column2, string body = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(InsertCardResourceQuery, new { CardId = cardId, ResourceType = resourceType }, transaction: transaction);
                    var newResourceId = await connection.QueryAsync<int>(GetLastCardResourceIdQuery, transaction: transaction); // todo: refactor on guid
                    var blobId = await connection.QueryAsync<int>(GetLastBlobIdQuery, transaction: transaction);
                    if (resourceType == ResourceType.Picture)
                        await connection.ExecuteAsync(InsertImageCardResourceQuery, new { BlobId = blobId.AsList()[0], Body = body, ResourceId = newResourceId.AsList()[0]}, transaction: transaction);
                    else
                        await connection.ExecuteAsync(InsertTableCardResourceQuery, new { Column1 = column1, Column2 = column2, ResourceId = newResourceId.AsList()[0] }, transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
