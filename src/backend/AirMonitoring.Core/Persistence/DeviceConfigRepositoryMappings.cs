using AirMonitoring.Core.Config;
using Amazon.DynamoDBv2.DocumentModel;

namespace AirMonitoring.Core.Persistence
{
    internal static class DeviceConfigRepositoryMappings
    {
        public static DeviceConfig ToDeviceConfig(this Document document)
        {
            return new DeviceConfig
            {
                DeviceId = document["DeviceId"].AsString(),
                ChatId = document["ChatId"].AsString(),
            };
        }
    }
}
