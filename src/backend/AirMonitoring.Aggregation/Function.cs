using AirMonitoring.Core.Extensions;
using AirMonitoring.Core.Persistence;
using Amazon.Lambda.Core;
using System.Text.Json.Nodes;


[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.Aggregation;

public class Function
{
    public async Task FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var deviceConfigsRepository = new DeviceConfigRepository(context.Logger);
        var measurementsRepository = new MeasurementsRepo(context.Logger);
        var aggregatedMeasurementsRepository = new AggregatedMeasurementsRepository(context.Logger);

        try
        {
            var devices = await deviceConfigsRepository.GetConfigs();

            foreach (var device in devices)
            {
                var from = DateTime.Today.AddDays(-1);
                var till = DateTime.Today;

                var records = await measurementsRepository.GetList(device.DeviceId, from, till);
                context.Logger.LogLine($"Found {records.Count} records.");

                var measurements = records
                    .Select(r => r.ToMeasurement());
                var aggregatedMeasurement = measurements.AggregateAverage();
                var aggregatedDbRecord = new MeasurementRecord(aggregatedMeasurement);

                await aggregatedMeasurementsRepository.Add(aggregatedDbRecord);
                context.Logger.LogLine($"Processed event: {from} for device: {device.DeviceId} successfully.");
            }
        }
        catch(Exception e)
        {
            context.Logger.LogLine($"ERROR processing aggregation of measurements: {e}");
        }
    }
}
