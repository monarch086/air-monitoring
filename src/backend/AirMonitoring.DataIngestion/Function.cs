using Amazon.Lambda.Core;
using System.Text.Json.Nodes;
using AirMonitoring.Core.Persistence;
using System.Text.Json;
using AirMonitoring.DataIngestion.Model;
using Amazon.Lambda.APIGatewayEvents;
using AirMonitoring.Core.HttpResponses;
using AirMonitoring.Core.HTTP;
using Newtonsoft.Json;
using Amazon.SimpleNotificationService.Model;
using AirMonitoring.Core.Model.Events.SNS;
using AirMonitoring.Core.Resources;
using Amazon.SimpleNotificationService;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirMonitoring.DataIngestion;

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(JsonObject input, ILambdaContext context)
    {
        try
        {
            var repository = new MeasurementsRepo(context.Logger);
            var snsClient = new AmazonSimpleNotificationServiceClient();

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
            context.Logger.LogLine($"Record [{requestBody}] was saved to database.");

            var message = new ValidationEvent
            {
                DeviceId = record.DeviceId,
                Date = record.Date
            };

            var messageJson = JsonConvert.SerializeObject(message);
            var publishRequest = new PublishRequest
            {
                TopicArn = SnsTopics.NewRecordAddedTopic,
                Message = messageJson,
                Subject = "New Record Added"
            };

            var response = await snsClient.PublishAsync(publishRequest);
            context.Logger.LogLine($"Message [{messageJson}] was published to SNS topic with ID: {response.MessageId}.");

            return new SuccessResponse("Data was successfully saved.");
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.ToString());
            return new FailResponse(e.ToString());
        }
    }
}
