namespace AirMonitoring.Queue
{
    public class QueueEvent
    {
        public string? Command { get; set; }

        public int ChatId { get; set; }

        public string[] Params { get; set; } = new string[0];
    }
}
