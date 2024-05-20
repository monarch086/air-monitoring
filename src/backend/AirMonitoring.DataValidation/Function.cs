using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Newtonsoft.Json;
using AirMonitoring.Core.Model.Events.SNS;
using AirMonitoring.Core.Persistence;
using AirMonitoring.Core.Resources;
using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataValidation;

public class Function
{
    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        var snsClient = new AmazonSimpleNotificationServiceClient();

        foreach (var record in snsEvent.Records)
        {
            var snsMessage = record.Sns.Message;
            context.Logger.LogLine($"Received SNS message: {snsMessage}");

            try
            {
                var message = JsonConvert.DeserializeObject<NewRecordEvent>(snsMessage);

                var repository = new MeasurementsRepository(context.Logger);
                var deviceId = message;

                var dbRecord = await repository.Get(message.DeviceId, message.Date);
                if (dbRecord == null)
                {
                    context.Logger.LogLine($"Could not find record in the database.");
                    return;
                }

                var measurement = dbRecord.ToMeasurement();

                if (!measurement.Validate())
                {
                    context.Logger.LogLine($"Measurement has NOT passed validation rules.");

                    await repository.Delete(message.DeviceId, message.Date);

                    return;
                }

                context.Logger.LogLine($"Measurement has passed validation rules.");

                var validationMessage = new ValidationEvent
                {
                    DeviceId = message.DeviceId,
                    Date = message.Date
                };

                var messageJson = JsonConvert.SerializeObject(validationMessage);
                var publishRequest = new PublishRequest
                {
                    TopicArn = SnsTopics.NewRecordValidatedTopic,
                    Message = messageJson,
                    Subject = "New Record Validated"
                };

                var response = await snsClient.PublishAsync(publishRequest);
                context.Logger.LogLine($"Message [{messageJson}] was published to SNS topic with ID: {response.MessageId}.");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error deserializing message: {ex.Message}");
            }
        }
    }
}
