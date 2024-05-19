using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;

namespace AirMonitoring.Core.Persistence
{
    public class MeasurementsRepo
    {
        private static string measurementsTableName = "SmartHome.Measurements";

        private readonly AmazonDynamoDBClient client;
        private readonly Table measurementsTable;
        private readonly ILambdaLogger logger;

        public MeasurementsRepo(ILambdaLogger logger)
        {
            client = new AmazonDynamoDBClient();
            measurementsTable = Table.LoadTable(client, measurementsTableName);
            this.logger = logger;
        }

        public async Task Add(MeasurementRecord record)
        {
            await measurementsTable.PutItemAsync(record.ToDocument());
        }

        public async Task<MeasurementRecord?> GetLatest(string deviceId)
        {
            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = "DeviceId = :v_deviceId";
            keyExpression.ExpressionAttributeValues[":v_deviceId"] = deviceId;

            var config = new QueryOperationConfig()
            {
                Limit = 1,
                Select = SelectValues.AllAttributes,
                BackwardSearch = true,
                ConsistentRead = true,
                KeyExpression = keyExpression
            };

            var queryResult = measurementsTable.Query(config);
            logger.LogInformation($"[GetLatest] Query result count: {queryResult.Count}");

            var documents = await queryResult.GetNextSetAsync();

            if (documents.Count > 0)
            {
                return documents[0].ToMeasurementRecord();
            }

            return null;
        }

        public async Task<MeasurementRecord?> Get(string deviceId, string date)
        {
            var document = await GetDocument(deviceId, date);

            return document != null ? document.ToMeasurementRecord() : null;
        }

        public async Task<bool> Delete(string deviceId, string date)
        {
            try
            {
                var document = await GetDocument(deviceId, date);
                if (document == null)
                {
                    logger.LogError($"[Delete] Document with key deviceId = {deviceId} and date = {date} not found.");
                    return false;
                }

                var result = await measurementsTable.DeleteItemAsync(document);

                logger.LogInformation($"[Delete] DeleteItem response: {result != null}");
                return result != null;
            }
            catch (Exception ex)
            {
                logger.LogError($"[Delete] Error deleting item: {ex.Message}");
                return false;
            }
        }

        public async Task<List<MeasurementRecord>> GetList(string deviceId, DateTime from, DateTime till)
        {
            var changes = new List<MeasurementRecord>();

            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = "DeviceId = :v_deviceId and #D between :v_start and :v_end";
            keyExpression.ExpressionAttributeValues[":v_deviceId"] = deviceId;
            keyExpression.ExpressionAttributeValues[":v_start"] = from.ToString(AWSSDKUtils.ISO8601DateFormat);
            keyExpression.ExpressionAttributeValues[":v_end"] = till.ToString(AWSSDKUtils.ISO8601DateFormat);
            keyExpression.ExpressionAttributeNames["#D"] = "Date";

            var config = new QueryOperationConfig()
            {
                Limit = 10,
                Select = SelectValues.AllAttributes,
                KeyExpression = keyExpression
            };

            var response = measurementsTable.Query(config);

            do
            {
                var documents = await response.GetNextSetAsync();
                foreach (var document in documents)
                {
                    changes.Add(document.ToMeasurementRecord());
                }
            } while (!response.IsDone);

            return changes
                .OrderBy(r => r.Date)
                .ToList();
        }

        public async Task<List<MeasurementRecord>> GetListWithCapacity(string deviceId, DateTime from, DateTime till)
        {
            var request = new QueryRequest
            {
                TableName = measurementsTableName,
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "DeviceId = :v_deviceId and #D between :v_start and :v_end",

                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_deviceId", new AttributeValue {
                         S = deviceId
                    }},
                    {":v_start", new AttributeValue {
                         S = from.ToString(AWSSDKUtils.ISO8601DateFormat)
                    }},
                    {":v_end", new AttributeValue {
                         S = till.ToString(AWSSDKUtils.ISO8601DateFormat)
                    }}
                },
                ExpressionAttributeNames = new Dictionary<string, string> {
                    {
                        "#D", "Date"
                    } }
            };

            var response = await client.QueryAsync(request);

            logger.LogInformation($"[GetList] Query result count: {response.Count}");
            logger.LogInformation($"[GetList] Consumed capacity - CapacityUnits: {response.ConsumedCapacity.CapacityUnits}");
            logger.LogInformation($"[GetList] Consumed capacity - ReadCapacityUnits: {response.ConsumedCapacity.ReadCapacityUnits}");
            logger.LogInformation($"[GetList] Consumed capacity - WriteCapacityUnits: {response.ConsumedCapacity.WriteCapacityUnits}");

            return response.Items
                .Select(i => i.ToMeasurementRecord())
                .OrderBy(r => r.Date)
                .ToList();
        }

        private async Task<Document?> GetDocument(string deviceId, string date)
        {
            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = "DeviceId = :v_deviceId and #D = :v_date";
            keyExpression.ExpressionAttributeValues[":v_deviceId"] = deviceId;
            keyExpression.ExpressionAttributeValues[":v_date"] = date;
            keyExpression.ExpressionAttributeNames["#D"] = "Date";

            var config = new QueryOperationConfig()
            {
                Limit = 1,
                Select = SelectValues.AllAttributes,
                BackwardSearch = true,
                ConsistentRead = true,
                KeyExpression = keyExpression
            };

            var queryResult = measurementsTable.Query(config);
            logger.LogInformation($"[GetLatest] Query result count: {queryResult.Count}");

            var documents = await queryResult.GetNextSetAsync();

            if (documents.Count > 0)
            {
                return documents[0];
            }

            return null;
        }
    }
}