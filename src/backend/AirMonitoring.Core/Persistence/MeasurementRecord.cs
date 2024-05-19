using AirMonitoring.Core.Model.MeasurementModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AirMonitoring.Core.Persistence
{
    public class MeasurementRecord
    {
        public string DeviceId { get; set; }

        public string Date { get; set; }

        public string JsonData { get; set; }

        public MeasurementRecord(Measurement measurement)
        {
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            DeviceId = measurement.DeviceId;
            Date = measurement.Date.ToString("O");
            JsonData = JsonSerializer.Serialize(measurement, options);
        }

        public MeasurementRecord() {}

        public Measurement ToMeasurement()
        {
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var measurement = JsonSerializer.Deserialize<Measurement>(JsonData, options) ?? new Measurement();
            measurement.DeviceId = DeviceId;
            measurement.Date = DateTime.Parse(Date);

            return measurement;
        }
    }
}
