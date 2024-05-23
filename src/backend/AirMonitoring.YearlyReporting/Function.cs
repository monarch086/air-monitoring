using AirMonitoring.Core.Config;
using AirMonitoring.Core.Model.Events.SQS;
using AirMonitoring.Core.Persistence;
using AirMonitoring.Core;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.YearlyReporting;

public class Function
{
    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        var config = await ConfigBuilder.Build(context.Logger);
        var bot = new ChatBot(config.Token);
        var repository = new AggregatedMeasurementsRepository(context.Logger);

        try
        {
            var payload = JsonSerializer.Deserialize<CommandEvent>(sqsEvent.Records[0].Body);
            if (payload == null || payload.Command == null)
                return;

            context.Logger.LogInformation($"Received command: {payload.Command}.");

            var range = TimeSpan.FromDays(365);
            var from = DateTime.UtcNow - range;
            var till = DateTime.UtcNow;
            var records = await repository.GetList(payload.DeviceId, from, till);
            var measurements = records
                .Select(r => r.ToMeasurement());

            var tempData = measurements
                .Select(m => m.Sht31?.Temperature)
                .Where(v => v != null)
                .ToArray();

            var tempChart = ChartGenerator.Generate(tempData, range, "t,°C");
            await bot.PostImageBytes(tempChart, "Recent year temperature measurements", payload.ChatId);

            var humidData = measurements
                .Select(m => m.Sht31?.Humidity)
                .Where(v => v != null)
                .ToArray();

            var humidChart = ChartGenerator.Generate(humidData, range, "%");
            await bot.PostImageBytes(humidChart, "Recent year humidity measurements", payload.ChatId);
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
        }
    }
}
