using AirMonitoring.Core.Model.Repository;
using Amazon.DynamoDBv2.DocumentModel;

namespace AirMonitoring.Core.Persistence
{
    internal static class DeviceConfigMappings
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
