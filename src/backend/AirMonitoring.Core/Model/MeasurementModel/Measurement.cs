using System.Text;
using System.Text.Json.Serialization;

namespace AirMonitoring.Core.Model.MeasurementModel
{
    public class Measurement
    {
        private const string DATE_TIME_FORMAT = @"dd.MM.yy H:mm";

        [JsonIgnore]
        public string DeviceId { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime Date { get; set; }

        [JsonPropertyName("BMP085")]
        public Bmp085Data? Bmp085 { get; set; }

        [JsonPropertyName("SHT31")]
        public Sht31Data? Sht31 { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Record date: {Date.ToString(DATE_TIME_FORMAT)}");

            if (Bmp085 != null)
            {
                sb.AppendLine($"[BMP180] Temperature: {Bmp085.Temperature} °C");
                sb.AppendLine($"[BMP180] Pressure: {Bmp085.Pressure} Pa");
            }

            if (Sht31 != null)
            {
                sb.AppendLine($"[SHT31] Temperature: {Sht31.Temperature} °C");
                sb.AppendLine($"[SHT31] Humidity: {Sht31.Humidity}%");
            }

            return sb.ToString();
        }

        public static Measurement Empty(DateTime dateTime)
        {
            return new Measurement()
            {
                Date = dateTime,
                Bmp085 = new Bmp085Data() { Temperature = "0" },
                Sht31 = new Sht31Data() { Temperature = "0", Humidity = "0" }
            };
        }
    }
}
