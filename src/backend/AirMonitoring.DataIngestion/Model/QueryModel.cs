using System.Text.Json.Serialization;

namespace AirMonitoring.DataIngestion.Model
{
    public class QueryModel
    {
        [JsonPropertyName("id")]
        public string DeviceId { get; set; } = string.Empty;
    }
}
