namespace AirMonitoring.CommandProcessing.Model.Telegram
{
    internal class CommandEvent
    {
        public int update_id { get; set; }

        public Message? message { get; set; }
    }
}
