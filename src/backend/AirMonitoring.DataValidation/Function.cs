using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json.Nodes;
using AirMonitoring.Core.HTTP;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataValidation;

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(JsonObject input, ILambdaContext context)
    {
        context.Logger.LogLine("Hello from AirMonitoring.DataAnalysis");

        return new SuccessResponse("Hello from AirMonitoring.DataAnalysis");
    }
}
