using AirMonitoring.Core.Extensions;
using AirMonitoring.Core.HTTP;
using AirMonitoring.Core.HttpResponses;
using AirMonitoring.Core.Model;
using AirMonitoring.Core.Persistence;
using AirMonitoring.DataProviding.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;
using System.Text.Json.Nodes;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataProviding;

public class Function
{
    private Dictionary<MeasurementType, Func<Measurement, string?>> dataSelectors = new Dictionary<MeasurementType, Func<Measurement, string?>>()
    {
        { MeasurementType.Temperature, m => m.Sht31?.Temperature },
        { MeasurementType.Humidity, m => m.Sht31?.Humidity },
        { MeasurementType.Pressure, m => m.Bmp085?.Pressure.ToString() }
    };

    public async Task<APIGatewayProxyResponse> FunctionHandler(JsonObject input, ILambdaContext context)
    {
        try
        {
            var query = input["queryStringParameters"].Deserialize<QueryModel>();
            if (query == null) { return new BadRequestResponse(); }

            var repository = new MeasurementsRepo(context.Logger);
            var deviceId = "S4D-12";

            var range = TimeSpan.FromHours(10);
            var from = DateTime.UtcNow - range;
            var till = DateTime.UtcNow;
            var records = await repository.GetList(deviceId, from, till);
            var measurements = records
                .Select(r => r.ToMeasurement());

            var filledMeasurements = measurements.FillAbsentMeasurements(from, till);

            var selector = dataSelectors[query.Type];
            if (selector == null)
            {
                return new FailResponse($"{query.Type} measurement type is not supported.");
            }

            var data = filledMeasurements
                .Select(selector)
                .Where(v => v != null)
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
