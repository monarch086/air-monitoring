using AirMonitoring.Core.Extensions;
using AirMonitoring.Core.Model.Events.SQS;
using AirMonitoring.Core.Persistence;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;


[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.Aggregation;

public class Function
{
    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        var deviceConfigsRepository = new DeviceConfigRepository(context.Logger);
        var measurementsRepository = new MeasurementsRepo(context.Logger);
        var aggregatedMeasurementsRepository = new AggregatedMeasurementsRepository(context.Logger);

        foreach (var record in sqsEvent.Records)
        {
            try
            {
                var payload = JsonSerializer.Deserialize<AggregationEvent>(record.Body);

                context.Logger.LogLine($"Received SQS message for event: {payload.MeasurementsDate}");

                var devices = await deviceConfigsRepository.GetConfigs();

                foreach (var device in devices)
                {
                    var range = TimeSpan.FromDays(1);
                    var from = DateTime.Parse(payload.MeasurementsDate);
                    var till = from + range;

                    var records = await measurementsRepository.GetList(device.DeviceId, from, till);
                    context.Logger.LogLine($"Found {records.Count} records.");

                    var measurements = records
                        .Select(r => r.ToMeasurement());
                    var aggregatedMeasurement = measurements.AggregateAverage();
                    var aggregatedDbRecord = new MeasurementRecord(aggregatedMeasurement);

                    await aggregatedMeasurementsRepository.Add(aggregatedDbRecord);
                    context.Logger.LogLine($"Processed event: {payload.MeasurementsDate} for device: {device.DeviceId} successfully.");
                }
            }
            catch(Exception e)
            {
                context.Logger.LogLine($"ERROR processing {record.Body}: {e}");
            }
        }
    }
}
