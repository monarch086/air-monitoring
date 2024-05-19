using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Newtonsoft.Json;
using AirMonitoring.Core.Model.Events.SNS;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataValidation;

public class Function
{
    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        foreach (var record in snsEvent.Records)
        {
            var snsMessage = record.Sns.Message;
            context.Logger.LogLine($"Received SNS message: {snsMessage}");

            try
            {
                var message = JsonConvert.DeserializeObject<NewRecordEvent>(snsMessage);


                // Process the deserialized message as needed
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error deserializing message: {ex.Message}");
            }
        }
    }
}
