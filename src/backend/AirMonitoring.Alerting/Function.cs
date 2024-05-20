using AirMonitoring.Core.Config;
using AirMonitoring.Core;
using AirMonitoring.Core.Model.Events.SNS;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Newtonsoft.Json;
using AirMonitoring.Core.Persistence;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.Alerting;

public class Function
{
    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        var deviceConfigRepo = new DeviceConfigRepository(context.Logger);

        foreach (var record in snsEvent.Records)
        {
            var snsMessage = record.Sns.Message;
            context.Logger.LogLine($"Received SNS message: {snsMessage}");

            try
            {
                var message = JsonConvert.DeserializeObject<AnomalyEvent>(snsMessage);
                var telegramConfig = await ConfigBuilder.Build(context.Logger);
                var bot = new ChatBot(telegramConfig.Token);

                var deviceConfig = (await deviceConfigRepo.GetConfigs())
                    .FirstOrDefault(c => c.DeviceId == message.DeviceId);

                if (deviceConfig != null && !string.IsNullOrEmpty(deviceConfig.ChatId))
                {
                    await bot.Post(message.Message, int.Parse(deviceConfig.ChatId));
                    context.Logger.LogLine($"Notification '{message.Message}' was sent to: {deviceConfig.ChatId} chat.");
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error processing message: {ex.Message}");
            }
        }
    }
}
