using AirMonitoring.Core.Config;
using AirMonitoring.Core.Model.Events.SQS;
using AirMonitoring.Core.Persistence;
using AirMonitoring.Core;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.LatestMeasurements;

public class Function
{
    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        var config = await ConfigBuilder.Build(context.Logger);
        var bot = new ChatBot(config.Token);
        var repository = new MeasurementsRepository(context.Logger);

        try
        {
            var payload = JsonSerializer.Deserialize<CommandEvent>(sqsEvent.Records[0].Body);
            if (payload == null || payload.Command == null)
                return;

            context.Logger.LogInformation($"Received command: {payload.Command}.");

            var lastMeasurement = (await repository.GetLatest(payload.DeviceId)).ToMeasurement();
            await bot.Post(lastMeasurement.ToString(), payload.ChatId);
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
        }
    }
}
