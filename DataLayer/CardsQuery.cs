using Dapper;
using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;

namespace Queries
{
    public sealed class CardsQuery
    {
        //string _connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CardsDB;Integrated Security=True;";
        //string _connectionString = @"Data Source=(localdb)\CardsDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //string _connectionString = @"Data Source=(localdb)\CardsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string _connectionString = @"Data Source = (localdb)\CardsDB;Initial Catalog = CardsDB; Integrated Security = True";
        //string _connectionString = @"Data Source=np:\\.\pipe\LOCALDB#049EA2DE\tsql\query;Initial Catalog=CardsDB;Integrated Security=True";

        const string QUERY = @"SELECT 
	                             C.Id AS CardID,
	                             C.[Name],
                                 C.IsReadOnly,
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
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cardsRelationships = await connection.QueryAsync<CardsRelationship>(QUERY);

                foreach (var item in cardsRelationships)
                {
                    if (item.ParentId == null)
                    {
                        result.Add(new Card { Id = item.CardID, Title = item.Name, Childs = new List<Card>() });
                        continue;
                    }

                    var parentCard = result.Single(x => x.Id == item.ParentId);
                    parentCard.Childs.Add(
                        new Card 
                        { 
                            Id = item.CardID, 
                            Title = item.Name,
                            InEditMode = item.IsReadOnly 
                        });
                }
            }
            return result.AsList();
        }
    }
}
