using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using AirMonitoring.Core.Model.Repository;

namespace AirMonitoring.Core.Persistence
{
    public class DeviceConfigRepository
    {
        private const string TABLE_NAME = "AirMonitoring.DeviceConfigs";

        private readonly AmazonDynamoDBClient client;
        private readonly Table configTable;
        private readonly ILambdaLogger logger;

        public DeviceConfigRepository(ILambdaLogger logger)
        {
            client = new AmazonDynamoDBClient();
            configTable = Table.LoadTable(client, TABLE_NAME);
            this.logger = logger;
        }

        public async Task<List<DeviceConfig>> GetConfigs()
        {
            var configs = new List<DeviceConfig>();

            var scanFilter = new ScanFilter();
            var scanResult = configTable.Scan(scanFilter);

            do
            {
                var documents = await scanResult.GetNextSetAsync();
                foreach (var document in documents)
                {
                    try
                    {
                        configs.Add(document.ToDeviceConfig());
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
            } while (!scanResult.IsDone);

            return configs;
        }
    }
}
