namespace AirMonitoring.Core.Extensions;

public static class TimeExtensions
{
    private static TimeZoneInfo KyivZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Kiev");

    public static DateTime FromKyivTime(this DateTime time)
    {
        return time.Subtract(KyivZone.BaseUtcOffset).ToUniversalTime();
    }

    public static DateTime ToKyivTime(this DateTime time)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(time, KyivZone);
    }
}
