using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Models;
using System.Linq;

namespace Commands
{
    public class UpdateCardResourceCommand
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;

        const string UpdateImageCardResourceQuery = @"UPDATE ImageCardsResources SET Body = @Body WHERE ResourceId = @ResourceId";
        
        public async Task ExecuteAsync(Card card)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var item in card.Resources.Where(x => x.ResourceType == ResourceType.Picture))
                    {
                        ImageResource typedItem = item as ImageResource;
                        if (typedItem == null)
                            continue;

                        await connection.ExecuteAsync(UpdateImageCardResourceQuery, new { ResourceId = item.Id, Body = typedItem.Description }, transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}