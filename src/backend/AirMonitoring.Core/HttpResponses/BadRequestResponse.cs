using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace AirMonitoring.Core.HttpResponses
{
    public class BadRequestResponse : APIGatewayProxyResponse
    {
        public BadRequestResponse()
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
            Headers = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Origin", "*" },
                { "Content-Type", "application/json" }
            };
        }
    }
}
