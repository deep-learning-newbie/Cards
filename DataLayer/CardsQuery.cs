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

        const string GetImageCardResourcesQuery = @"SELECT
	                                                    R.Id,
	                                                    R.CardId,
	                                                    R.ResourceType,
	                                                    IR.Body,
	                                                    B.Data
                                                    FROM CardsResources R
	                                                    INNER JOIN ImageCardsResources IR ON R.Id = IR.ResourceId
	                                                    INNER JOIN Blobs B ON B.Id = IR.BlobId
                                                    WHERE R.CardId = @CardId";

        const string GetTableCardResourcesQuery = @"SELECT
	                                                    R.Id,
	                                                    R.CardId,
	                                                    R.ResourceType,
                                                        TR.Column1,
                                                        TR.Column2
                                                    FROM CardsResources R
	                                                    INNER JOIN TableCardsResources TR ON R.Id = TR.ResourceId
                                                    WHERE R.CardId = @CardId";

        private class CardsRelationship
        {
            public int CardId { get; set; }
            public string Name { get; set; }
            public bool IsReadOnly { get; set; }
            public int? ParentId { get; set; }
            public int? ChildId { get; set; }
        }

        private class ImageCardResource
        {
            public int Id { get; set; }
            public int CardId { get; set; }
            public ResourceType ResourceType { get; set; }
            public string Body { get; set; }
            public byte[] Data { get; set; }
        }

        private class TableCardResource
        {
            public int Id { get; set; }
            public int CardId { get; set; }
            public ResourceType ResourceType { get; set; }
            public string Column1 { get; set; }
            public string Column2 { get; set; }
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
                    // Create parent card
                    if (item.ParentId == null)
                    {
                        var parentCard = new Card { Id = item.CardId, Title = item.Name, Childs = new List<Card>() };
                        result.Add(parentCard);

                        // Work with resources
                        var imageResources = await connection.QueryAsync<ImageCardResource>(GetImageCardResourcesQuery, new { CardId = item.CardId });
                        var tableResources = await connection.QueryAsync<TableCardResource>(GetTableCardResourcesQuery, new { CardId = item.CardId });

                        foreach (var imageResource in imageResources)
                            parentCard.Resources.Add(new ImageResource { Id = imageResource.Id, Index = 1, Description = imageResource.Body, Uri = "340719-200.png", Data = imageResource.Data });
                        foreach (var tableResource in tableResources)
                        {
                            parentCard.Resources.Add(new TableResource
                            {
                                Id = tableResource.Id,
                                Index = 1,
                                Rows = new List<TableResourceItem>()
                                    {
                                        new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" },
                                        new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }
                                    }
                            });
                        }
                    }
                    else
                    {
                        // Add child card
                        var parentCard = result.Single(x => x.Id == item.ParentId);
                        var childCard = new Card
                        {
                            Id = item.CardId,
                            Title = item.Name,
                            InEditMode = !item.IsReadOnly // TODO: What for we use this property?
                        };
                        parentCard.Childs.Add(childCard);

                        // Work with resources
                        var imageResources = await connection.QueryAsync<ImageCardResource>(GetImageCardResourcesQuery, new { CardId = item.CardId });
                        var tableResources = await connection.QueryAsync<TableCardResource>(GetTableCardResourcesQuery, new { CardId = item.CardId });

                        foreach (var imageResource in imageResources)
                            childCard.Resources.Add(new ImageResource { Id = imageResource.Id, Index = 1, Description = imageResource.Body, Uri = "340719-200.png", Data = imageResource.Data });
                        foreach (var tableResource in tableResources)
                        {
                            childCard.Resources.Add(new TableResource
                            {
                                Id = tableResource.Id,
                                Index = 1,
                                Rows = new List<TableResourceItem>()
                                    {
                                        new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" },
                                        new TableResourceItem() { Column1 = "Item 1", Column2 = "Item 2" }
                                    }
                            });
                        }

                    }
                }
            }
            return result.AsList();
        }
    }
}
