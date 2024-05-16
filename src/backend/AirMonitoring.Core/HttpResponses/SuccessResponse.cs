﻿using Amazon.Lambda.APIGatewayEvents;
using System.Net;

namespace AirMonitoring.Core.HTTP
{
    public class SuccessResponse : APIGatewayProxyResponse
    {
        public SuccessResponse(string body)
        {
            StatusCode = (int)HttpStatusCode.OK;
            Headers = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Origin", "*" },
                { "Content-Type", "application/json" }
            };
            Body = body;
        }
    }
}
