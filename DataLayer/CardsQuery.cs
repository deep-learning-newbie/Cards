using Dapper;
using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;

namespace Queries
{
    public sealed class CardsQuery
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["CardsDBConnectionString"].ConnectionString;

        const string GetCardsQuery = @"SELECT 
	                                     C.Id AS CardID,
	                                     C.[Name],
                                         C.InEditMode,
	                                     CR.ParentId,
	                                     CR.ChildId
                                       FROM Cards C
	                                     LEFT JOIN CardsRelationship CR ON C.Id = CR.ChildId
                                       ORDER BY CR.ParentId";

        const string GetCardResourcesQuery = @"SELECT 
	                                             C.Id AS CardID,
	                                             C.[Name],
                                                 C.InEditMode,
	                                             CR.ParentId,
	                                             CR.ChildId
                                               FROM Cards C
	                                             LEFT JOIN CardsRelationship CR ON C.Id = CR.ChildId
                                               ORDER BY CR.ParentId";

        private class CardsRelationship
        {
            public int CardID { get; set; }
            public string Name { get; set; }
            public bool IsReadOnly { get; set; }
            public int? ParentId { get; set; }
            public int? ChildId { get; set; }
        }

        public async Task<List<Card>> ExecuteAsync()
        {
            List<Card> result = new List<Card>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var cardsRelationships = await connection.QueryAsync<CardsRelationship>(GetCardsQuery);

                foreach (var item in cardsRelationships)
                {
                    if (item.ParentId == null)
                    {
                        result.Add(new Card { Id = item.CardID, Title = item.Name, Childs = new List<Card>() });

                        // Work with resources
                        // var resources = 
                        // card21.Resources.Add(new TableResource() { Index = 2, Rows = new List<TableResourceItem>() { new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }, new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" } } });
                        // card21.Resources.Add(new ImageResource() { Index = 3, Description = "some text", Uri = "340719-200.png" });

                        continue;
                    }

                    var parentCard = result.Single(x => x.Id == item.ParentId);
                    parentCard.Childs.Add(
                        new Card 
                        { 
                            Id = item.CardID, 
                            Title = item.Name,
                            InEditMode = item.IsReadOnly // TODO: What for we use this property?
                        });


                }
            }
            return result.AsList();
        }
    }
}
