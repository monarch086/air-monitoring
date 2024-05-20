using AirMonitoring.Core.HTTP;
using AirMonitoring.Core.HttpResponses;
using AirMonitoring.Core.Model.MeasurementModel;
using AirMonitoring.Core.Persistence;
using AirMonitoring.DataProviding.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataProviding;

public class Function
{
    private Dictionary<MeasurementType, Func<Measurement, MeasurementItem>> dataSelectors = new Dictionary<MeasurementType, Func<Measurement, MeasurementItem>>()
    {
        { MeasurementType.Temperature, m => new MeasurementItem { Date = m.Date, Value = m.Sht31?.Temperature } },
        { MeasurementType.Humidity, m => new MeasurementItem { Date = m.Date, Value = m.Sht31?.Humidity } },
        { MeasurementType.Pressure, m => new MeasurementItem { Date = m.Date, Value = m.Bmp085?.Pressure.ToString() } },
    };

    public async Task<APIGatewayProxyResponse> FunctionHandler(JsonObject input, ILambdaContext context)
    {
        try
        {
            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };
            options.Converters.Add(new JsonStringEnumConverter());

            var query = input["queryStringParameters"].Deserialize<QueryModel>(options);
            if (query == null) { return new BadRequestResponse(); }

            var repository = query.Days > 5 
                ? new AggregatedMeasurementsRepository(context.Logger)
                : new MeasurementsRepo(context.Logger);

            var deviceId = "S4D-12";

            var range = TimeSpan.FromDays(query.Days);
            var from = DateTime.UtcNow - range;
            var till = DateTime.UtcNow;
            var records = await repository.GetList(deviceId, from, till);
            var measurements = records
                .Select(r => r.ToMeasurement());

            var selector = dataSelectors[query.Type];
            if (selector == null)
            {
                return new FailResponse($"{query.Type} measurement type is not supported.");
            }

            var data = measurements
                .Select(selector)
                .Where(m => m.Value != null)
                .ToArray();

            return new SuccessResponse(JsonSerializer.Serialize(data));
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
            return new FailResponse(e.ToString());
        }
    }
}
