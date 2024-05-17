using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace service_users
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(JsonObject input, ILambdaContext context)
        {
            var users = new User[] {
                new User { Id = 4, FirstName = "Tom", LastName = "Smith" },
                new User { Id = 5, FirstName = "James", LastName = "Ford" },
                new User { Id = 6, FirstName = "Nick", LastName = "Wilde" },
            };

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Content-Type", "application/json" }
                },
                Body = JsonSerializer.Serialize(users)
            };
        }
    }
}
