namespace AirMonitoring.Core.Resources
{
    public static class SqsQueues
    {
        public static string AggregationQueue = $"https://sqs.{AccountSettings.Region}.amazonaws.com/{AccountSettings.AccountId}/air-monitoring-aggregation-queue";

        public static string MonthlyReportQueue = $"https://sqs.{AccountSettings.Region}.amazonaws.com/{AccountSettings.AccountId}/air-monitoring-monthly-report-queue";

        public static string YearlyReportQueue = $"https://sqs.{AccountSettings.Region}.amazonaws.com/{AccountSettings.AccountId}/air-monitoring-yearly-report-queue";
    }
}
