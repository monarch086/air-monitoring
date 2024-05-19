namespace AirMonitoring.Core.Resources
{
    public static class SnsTopics
    {
        public static string NewRecordAddedTopic = $"arn:aws:sns:{AccountSettings.Region}:{AccountSettings.AccountId}:air-monitoring-record-added";

        public static string NewRecordValidatedTopic = $"arn:aws:sns:{AccountSettings.Region}:{AccountSettings.AccountId}:air-monitoring-record-validated";

        public static string AnomalyDetectedTopic = $"arn:aws:sns:{AccountSettings.Region}:{AccountSettings.AccountId}:air-monitoring-anomaly-detected";
    }
}
