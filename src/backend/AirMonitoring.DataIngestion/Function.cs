using Amazon.Lambda.Core;
using System.Text.Json.Nodes;
using AirMonitoring.Core.Persistence;
using System.Text.Json;
using AirMonitoring.DataIngestion.Model;
using Amazon.Lambda.APIGatewayEvents;
using AirMonitoring.Core.HttpResponses;
using AirMonitoring.Core.HTTP;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataIngestion;

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(JsonObject input, ILambdaContext context)
    {
        try
        {
            var repository = new MeasurementsRepo(context.Logger);

            var query = input["queryStringParameters"].Deserialize<QueryModel>();
            if (query == null) { return new BadRequestResponse(); }

            var requestBody = input["body"];
            if (requestBody == null) { return new BadRequestResponse(); }

            context.Logger.LogLine(requestBody.ToString());

            var record = new MeasurementRecord
            {
                DeviceId = query.DeviceId,
                Date = DateTime.UtcNow.ToString("O"),
                JsonData = requestBody.ToString()
            };

            await repository.Add(record);

            return new SuccessResponse("Data was successfully saved.");
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
            return new FailResponse(e.ToString());
        }
    }
}
