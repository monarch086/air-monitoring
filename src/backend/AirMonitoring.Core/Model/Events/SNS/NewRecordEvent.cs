namespace AirMonitoring.Core.Model.Events.SNS
{
    public class NewRecordEvent
    {
        public string DeviceId { get; set; } = string.Empty;

        public string Date { get; set; }
    }
}
