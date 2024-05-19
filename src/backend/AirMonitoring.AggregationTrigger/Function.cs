using AirMonitoring.Core.Model.Events.SQS;
using AirMonitoring.Core.Resources;
using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using System.Text.Json.Nodes;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.AggregationTrigger;

public class Function
{
    public async Task FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var amazonSQSClient = new AmazonSQSClient();
        var sendRequest = new SendMessageRequest();
        sendRequest.QueueUrl = SqsQueues.AggregationQueue;

        var from = DateTime.Parse("2023-02-01");

        while (from < DateTime.UtcNow)
        {
            var queueEvent = new AggregationEvent
            {
                MeasurementsDate = from.ToString("O")
            };

            sendRequest.MessageBody = JsonSerializer.Serialize(queueEvent);
            var sendMessageResponse = await amazonSQSClient.SendMessageAsync(sendRequest);

            from = from.AddDays(1);

            context.Logger.LogLine($"Send command to queue - response HTTP Status Code: {sendMessageResponse.HttpStatusCode}.");
        }
    }
}