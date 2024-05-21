namespace AirMonitoring.Core.Model.Events.SQS
{
    public class CommandEvent
    {
        public string Command { get; set; }

        public int ChatId { get; set; }

        public string DeviceId { get; set; }

        public string[] Params { get; set; } = new string[0];
    }
}
