using AirMonitoring.Core.Model.MeasurementModel;

namespace AirMonitoring.Core.Extensions;

public static class MeasurementsExtensions
{
    private static TimeSpan DefaultInterval = TimeSpan.FromMinutes(10);

    //Fills absent measurements with empty values
    public static IEnumerable<Measurement> FillAbsentMeasurements(this IEnumerable<Measurement> measurements, DateTime from, DateTime till, TimeSpan interval)
    {
        var result = new List<Measurement>();

        var currentTimePoint = from;

        foreach (var measurement in measurements)
        {
            while ((measurement.Date - currentTimePoint).TotalMinutes > (interval.TotalMinutes + 1))
            {
                var emptyMeasurement = Measurement.Empty(currentTimePoint);
                result.Add(emptyMeasurement);
                currentTimePoint = currentTimePoint.Add(interval);
            }

            result.Add(measurement);
            currentTimePoint = measurement.Date;
        }

        while (!result.Any() || (till - result.Last().Date).TotalMinutes > (interval.TotalMinutes + 1))
        {
            var emptyMeasurement = Measurement.Empty(currentTimePoint);
            result.Add(emptyMeasurement);
            currentTimePoint = currentTimePoint.Add(interval);
        }

        return result;
    }

    public static IEnumerable<Measurement> FillAbsentMeasurements(this IEnumerable<Measurement> measurements, DateTime from, DateTime till)
        => measurements.FillAbsentMeasurements(from, till, DefaultInterval);
}
