using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace AirMonitoring.Core.Persistence
{
    internal static class DbMappings
    {
        public static Document ToDocument(this MeasurementRecord record)
        {
            var attributes = new Dictionary<string, AttributeValue>()
              {
                  { "DeviceId", new AttributeValue { S = record.DeviceId }},
                  { "Date", new AttributeValue { S = record.Date }},
                  { "JsonData", new AttributeValue { S = record.JsonData }},
              };

            return Document.FromAttributeMap(attributes);
        }

        public static MeasurementRecord ToMeasurementRecord(this Document document)
        {
            return new MeasurementRecord()
            {
                DeviceId = document["DeviceId"].AsString(),
                Date = document["Date"].AsString(),
                JsonData = document["JsonData"].AsString()
            };
        }

        public static MeasurementRecord ToMeasurementRecord(this Dictionary<string, AttributeValue> data)
        {
            var document = Document.FromAttributeMap(data);
            return document.ToMeasurementRecord();
        }
    }
}
