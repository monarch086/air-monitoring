using Amazon.Lambda.Core;
using System.Text.Json.Nodes;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using AirMonitoring.CommandProcessing.Model.Telegram;
using AirMonitoring.Core.Queue;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.CommandProcessing;

public class Function
{
    public async Task FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var requestBody = input["body"];
        if (requestBody == null) { return; }

        context.Logger.LogLine(requestBody.ToString());

        var commandEvent = JsonSerializer.Deserialize<CommandEvent>(requestBody.ToString());
        var command = commandEvent?.message?.text;
        context.Logger.LogLine($"Command: {command}");

        // Publish to SQS Queue
        var queueUrl = "https://sqs.eu-central-1.amazonaws.com/469321902251/SmartHome-Status";
        context.Logger.LogLine($"Send command to queue: {queueUrl}.");
        var queueEvent = new QueueEvent
        {
            Command = command,
            ChatId = commandEvent?.message?.chat?.id ?? 0,
            Params = new string[] { "DeviceId:S4D-12" }
        };

        var amazonSQSClient = new AmazonSQSClient();
        var sendRequest = new SendMessageRequest();
        sendRequest.QueueUrl = queueUrl;
        sendRequest.MessageBody = JsonSerializer.Serialize(queueEvent);
        var sendMessageResponse = await amazonSQSClient.SendMessageAsync(sendRequest);

        context.Logger.LogLine($"Send command to queue - response HTTP Status Code: {sendMessageResponse.HttpStatusCode}.");
    }
}
