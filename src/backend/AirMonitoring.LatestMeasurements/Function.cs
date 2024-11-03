using AirMonitoring.Core.Config;
using AirMonitoring.Core.Model.Events.SQS;
using AirMonitoring.Core.Persistence;
using AirMonitoring.Core;
using AirMonitoring.Core.Extensions;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.LatestMeasurements;

public class Function
{
    public async Task FunctionHandler(object inputEvent, ILambdaContext context)
    {
        var config = await ConfigBuilder.Build(context.Logger);
        var bot = new ChatBot(config.Token);
        var repository = new MeasurementsRepository(context.Logger);

        try
        {
            var payload = inputEvent is SQSEvent sqsEvent && sqsEvent.Records.Count > 0
                ? GetPayloadFromSqsEvent(sqsEvent, context)
                : GetPayloadFromCronEvent();

            if (payload != null)
            {
                var range = TimeSpan.FromHours(10);
                var from = DateTime.UtcNow - range;
                var till = DateTime.UtcNow;
                var records = await repository.GetList(payload.DeviceId, from, till);
                var measurements = records
                    .Select(r => r.ToMeasurement());

                var filledMeasurements = measurements.FillAbsentMeasurements(from, till);

                var tempData = filledMeasurements
                    .Select(m => m.Sht31?.Temperature)
                    .Where(v => v != null)
                    .ToArray();

                var chart = ChartGenerator.Generate(tempData, range, "t,°C");

                var lastMeasurement = measurements.Last();
                await bot.PostImageBytes(chart, lastMeasurement.ToString(), payload.ChatId);
            }
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
        }
    }

    private CommandEvent? GetPayloadFromSqsEvent(SQSEvent sqsEvent, ILambdaContext context)
    {
        var payload = JsonSerializer.Deserialize<CommandEvent>(sqsEvent.Records[0].Body);
        return payload;
    }

    private CommandEvent GetPayloadFromCronEvent()
    {
        return new CommandEvent
        {
            ChatId = 38627946,
            DeviceId = "S4D-12"
        };
    }
}
