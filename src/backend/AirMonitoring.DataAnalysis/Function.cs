using AirMonitoring.Core.Extensions;
using AirMonitoring.Core.Model.Events.SNS;
using AirMonitoring.Core.Persistence;
using AirMonitoring.Core.Resources;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataAnalysis;

public class Function
{
    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        var snsClient = new AmazonSimpleNotificationServiceClient();
        var repository = new MeasurementsRepository(context.Logger);

        foreach (var record in snsEvent.Records)
        {
            var snsMessage = record.Sns.Message;
            context.Logger.LogLine($"Received SNS message: {snsMessage}");

            try
            {
                var message = JsonConvert.DeserializeObject<ValidationEvent>(snsMessage);

                var dbRecord = await repository.Get(message.DeviceId, message.Date);
                if (dbRecord == null)
                {
                    context.Logger.LogLine($"Could not find record in the database.");
                    return;
                }

                var measurement = dbRecord.ToMeasurement();

                var till = DateTime.Parse(message.Date);
                var from = till.AddHours(-3);
                var previousMeasurements = (await repository.GetList(message.DeviceId, from, till))
                    .Select(r => r.ToMeasurement());
                var aggregated = previousMeasurements.AggregateAverage();

                var anomaly = measurement.HasAnomaly(aggregated);

                if (anomaly != null)
                {
                    context.Logger.LogLine($"Measurement anomaly {anomaly} was detected.");

                    var anomalyMessage = new AnomalyEvent
                    {
                        DeviceId = message.DeviceId,
                        Date = message.Date,
                        Type = anomaly.Value,
                        Message = AnomalyMessageGenerator.Generate(anomaly.Value, measurement)
                    };

                    var messageJson = JsonConvert.SerializeObject(anomalyMessage);
                    var publishRequest = new PublishRequest
                    {
                        TopicArn = SnsTopics.AnomalyDetectedTopic,
                        Message = messageJson,
                        Subject = "Anomaly Detected"
                    };

                    var response = await snsClient.PublishAsync(publishRequest);
                    context.Logger.LogLine($"Message [{messageJson}] was published to SNS topic with ID: {response.MessageId}.");
                }

                context.Logger.LogLine($"No measurement anomalies were detected.");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error processing message: {ex.Message}");
            }
        }
    }
}
