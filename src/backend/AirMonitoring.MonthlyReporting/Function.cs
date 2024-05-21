using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AirMonitoring.Core.Config;
using AirMonitoring.Core;
using System.Text.Json;
using AirMonitoring.Core.Model.Events.SQS;
using AirMonitoring.Core.Persistence;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.MonthlyReporting;

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

            var range = TimeSpan.FromHours(10);
            var from = DateTime.UtcNow - range;
            var till = DateTime.UtcNow;
            var records = await repository.GetList(payload.DeviceId, from, till);
            var measurements = records
                .Select(r => r.ToMeasurement());

           // var filledMeasurements = measurements.FillAbsentMeasurements(from, till);

            var tempData = measurements
                .Select(m => m.Sht31?.Temperature)
                .Where(v => v != null)
                .ToArray();

            var chart = ChartGenerator.Generate(tempData, range);
            var lastMeasurement = measurements.Last();
            await bot.PostImageBytes(chart, lastMeasurement.ToString(), payload.ChatId);
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
        }
    }
}
