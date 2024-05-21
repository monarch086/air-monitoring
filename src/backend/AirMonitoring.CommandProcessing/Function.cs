using Amazon.Lambda.Core;
using System.Text.Json.Nodes;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using AirMonitoring.Core.HttpResponses;
using AirMonitoring.Core.HTTP;
using Amazon.Lambda.APIGatewayEvents;
using AirMonitoring.Core.Resources;
using AirMonitoring.Core.Persistence;
using AirMonitoring.Core.Model.Events.SQS;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.CommandProcessing;

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var deviceConfigRepo = new DeviceConfigRepository(context.Logger);

        try
        {
            var requestBody = input["body"];
            if (requestBody == null) { return new BadRequestResponse(); }

            context.Logger.LogLine(requestBody.ToString());

            var commandEvent = JsonSerializer.Deserialize<Model.Telegram.CommandEvent>(requestBody.ToString());
            var command = commandEvent?.message?.text;
            context.Logger.LogLine($"Received command: {command}");

            var queueUrl = resolveQueue(command);
            if (string.IsNullOrEmpty(queueUrl))
            {
                return new BadRequestResponse($"Command {command} is not supported.");
            }

            var chatId = commandEvent?.message?.chat?.id;
            if (!chatId.HasValue)
            {
                return new FailResponse("Could not get chatId from incoming message.");
            }

            var deviceId = await getDeviceId(chatId.Value, deviceConfigRepo);
            if (string.IsNullOrEmpty(deviceId))
            {
                return new FailResponse("Could not get deviceId.");
            }

            var queueEvent = new CommandEvent
            {
                Command = command,
                ChatId = chatId.Value,
                DeviceId = deviceId
            };

            var amazonSQSClient = new AmazonSQSClient();
            var sendRequest = new SendMessageRequest();
            sendRequest.QueueUrl = queueUrl;
            sendRequest.MessageBody = JsonSerializer.Serialize(queueEvent);
            var sendMessageResponse = await amazonSQSClient.SendMessageAsync(sendRequest);

            context.Logger.LogLine($"Sent command to queue {queueUrl} - response HTTP Status Code: {sendMessageResponse.HttpStatusCode}.");

            return new SuccessResponse("Command was successfully published.");
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
            return new FailResponse(e.ToString());
        }
    }

    private string resolveQueue(string command)
    {
        switch (command)
        {
            case Commands.MONTHLY_REPORT: return SqsQueues.MonthlyReportQueue;
            case Commands.YEARLY_REPORT: return SqsQueues.YearlyReportQueue;
            default: return string.Empty;
        }
    }

    private async Task<string> getDeviceId(int chatId, DeviceConfigRepository repository)
    {
        var deviceConfig = (await repository.GetConfigs())
                    .FirstOrDefault(c => int.Parse(c.ChatId) == chatId);

        return deviceConfig != null ? deviceConfig.DeviceId : string.Empty;
    }
}
