using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Azure.Identity;
using System.Linq;

namespace VolcanoAPI
{
    public class DataService
    {
        private static readonly ChainedTokenCredential tokenCredential = new ChainedTokenCredential
        (
            new AzureCliCredential(),
            new ManagedIdentityCredential()
        );

        private static string EndpointUrl = "https://az-fun-demo-cm.documents.azure.com";

        private static readonly CosmosClient cosmosClient = new CosmosClient(EndpointUrl, tokenCredential);

        public static async Task<List<Volcano>> GetVolcanos()
        {
            QueryRequestOptions options = new QueryRequestOptions() { MaxBufferedItemCount = 100 };
            var volcanos = new List<Volcano>();
            var database = cosmosClient.GetDatabase("VolcanoList");
            var container = database.GetContainer("Volcanos");
            var queryText = "SELECT * FROM Volcanos";
            using (FeedIterator<Volcano> query = container.GetItemQueryIterator<Volcano>(
                queryText,
                requestOptions: options))
            {
                while (query.HasMoreResults)
                {
                    foreach (var volcano in await query.ReadNextAsync())
                    {
                        volcanos.Add(volcano);
                    }
                }
            }

            return volcanos;
        }

        public static async Task<Volcano> GetVolcano(string name)
        {
            QueryDefinition query = new QueryDefinition(
                "select * from volcanos s where s.VolcanoName = @volcanoName ")
                .WithParameter("@volcanoName", "name");
            var database = cosmosClient.GetDatabase("VolcanoList");
            var container = database.GetContainer("Volcano");
            var volcanos = new List<Volcano>();
            using (FeedIterator<Volcano> resultSet = container.GetItemQueryIterator<Volcano>(
               query,
               requestOptions: new QueryRequestOptions()
               {
                   PartitionKey = new PartitionKey("id"),
                   MaxItemCount = 1
               }))
            {
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<Volcano> response = await resultSet.ReadNextAsync();
                    volcanos.AddRange(response);
                }
            }
            return volcanos.FirstOrDefault();
        }
    }
}
