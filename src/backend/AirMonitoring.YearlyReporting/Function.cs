using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json.Nodes;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.YearlyReporting;

public class Function
{
    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        context.Logger.LogLine("Hello from AirMonitoring.YearlyReporting");
    }
}
