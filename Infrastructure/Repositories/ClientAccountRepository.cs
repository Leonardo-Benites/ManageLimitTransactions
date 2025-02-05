using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Domain.Models;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
namespace Infrastructure.Repositories
{
    public class ClientAccountRepository : IClientAccountRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly IOptions<DatabaseSettings> _databaseSettings;
        public ClientAccountRepository(IAmazonDynamoDB db, IOptions<DatabaseSettings> dbSettings)
        {
            _dynamoDb = db;
            _databaseSettings = dbSettings;
        }

        public async Task<bool> CreateAsync(ClientAccount obj)
        {
            var customerAsJson = JsonSerializer.Serialize(obj);
            var itemAsDocument = Document.FromJson(customerAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();

            var createItemRequest = new PutItemRequest
            {
                TableName = _databaseSettings.Value.TableName,
                Item = itemAsAttributes
            };

            var response = await _dynamoDb.PutItemAsync(createItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<ClientAccount?> GetByAccountAsync(string accountNumber)
        {
            var request = new GetItemRequest
            {
                TableName = _databaseSettings.Value.TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "accountNumber", new AttributeValue { S = accountNumber } }, 
                }
            };

            var response = await _dynamoDb.GetItemAsync(request);
            if (response.Item.Count == 0)
            {
                return null;
            }

            var itemAsDocument = Document.FromAttributeMap(response.Item);
            return JsonSerializer.Deserialize<ClientAccount>(itemAsDocument.ToJson());
        }

        public async Task<bool> UpdateLimitAsync(string accountNumber, decimal newTransactionLimit)
        {
            var updateItemRequest = new UpdateItemRequest
            {
                TableName = _databaseSettings.Value.TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "accountNumber", new AttributeValue { S = accountNumber } } 
                },
                UpdateExpression = "SET transactionLimit = :newLimit",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":newLimit", new AttributeValue { N = newTransactionLimit.ToString() } }
                }
            };

            var response = await _dynamoDb.UpdateItemAsync(updateItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> UpdateLimitBalanceAsync(string accountNumber, decimal newBalance)
        {
            var updateItemRequest = new UpdateItemRequest
            {
                TableName = _databaseSettings.Value.TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "accountNumber", new AttributeValue { S = accountNumber } }
                },
                UpdateExpression = "SET balance = :newBalance",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":newBalance", new AttributeValue { N = newBalance.ToString() } }
                }
            };

            var response = await _dynamoDb.UpdateItemAsync(updateItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteAsync(string accountNumber)
        {
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = _databaseSettings.Value.TableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "accountNumber", new AttributeValue { S = accountNumber } },
            }
            };

            var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
