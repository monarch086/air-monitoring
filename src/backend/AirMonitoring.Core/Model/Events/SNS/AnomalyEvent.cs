using AirMonitoring.Core.Model.Anomaly;

namespace AirMonitoring.Core.Model.Events.SNS
{
    public class AnomalyEvent
    {
        public string DeviceId { get; set; } = string.Empty;

        public string Date { get; set; }

        public AnomalyType Type { get; set; }

        public string Message { get; set; }
    }
}
