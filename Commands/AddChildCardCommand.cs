using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Commands
{
    public class AddChildCardCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;
        
        const string InsertCardQuery = @"INSERT INTO Cards (Title) VALUES (@Title)";
        const string GetLastCardIdQuery = @"SELECT MAX(Id) FROM Cards";
        const string InsertCardRelationshipQuery = @"INSERT INTO CardsRelationship(ParentId, ChildId) VALUES(@ParentId, @ChildId)";

        public async Task ExecuteAsync(int parentCardId, string title)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(InsertCardQuery, new { Title = title }, transaction: transaction);
                    var newId = await connection.QueryAsync<int>(GetLastCardIdQuery, transaction: transaction); // todo: refactor on guid
                    await connection.ExecuteAsync(InsertCardRelationshipQuery, new { ParentId = parentCardId, ChildId = newId.AsList()[0]}, transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
