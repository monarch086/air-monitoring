using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.SimpleSystemsManagement;

namespace AirMonitoring.Core.Config;

public class ConfigBuilder
{
    private static string TOKEN_PARAM_NAME = "SmartHome.TelegramBot.Token";

    public static async Task<TelegramConfig> Build(ILambdaLogger logger)
    {
        try
        {
            var config = new TelegramConfig();

            var client = new AmazonSimpleSystemsManagementClient();

            var request = new GetParameterRequest()
            {
                Name = $"{TOKEN_PARAM_NAME}"
            };
            var result = await client.GetParameterAsync(request);
            config.Token = result.Parameter.Value;

            return config;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());

            return new TelegramConfig();
        }
    }
}
