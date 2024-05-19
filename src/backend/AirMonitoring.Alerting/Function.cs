using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleNotificationService;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.Alerting;

public class Function
{
    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        var snsClient = new AmazonSimpleNotificationServiceClient();

        foreach (var record in snsEvent.Records)
        {
            var snsMessage = record.Sns.Message;
            context.Logger.LogLine($"Received SNS message: {snsMessage}");
        }
    }
}
