using Amazon.Lambda.Core;
using System.Text.Json.Nodes;
using AirMonitoring.Core.Persistence;
using System.Text.Json;
using AirMonitoring.DataIngestion.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataIngestion;

public class Function
{
    public async Task FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var repository = new MeasurementsRepo(context.Logger);

        var query = input["queryStringParameters"].Deserialize<QueryModel>();
        if (query == null) { return; }

        var requestBody = input["body"];
        if (requestBody == null) { return; }

        context.Logger.LogLine(requestBody.ToString());

        try
        {
            var record = new MeasurementRecord
            {
                DeviceId = query.DeviceId,
                Date = DateTime.UtcNow.ToString("O"),
                JsonData = requestBody.ToString()
            };

            await repository.Add(record);
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
        }
    }
}
