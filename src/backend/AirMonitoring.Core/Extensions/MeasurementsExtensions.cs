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

    public static Measurement AggregateAverage(this IEnumerable<Measurement> measurements)
    {
        var bmp085Temp = measurements.Select(m => m.Bmp085?.Temperature ?? string.Empty).Average();
        var bmp085Press = measurements.Select(m => m.Bmp085?.Pressure).Average();
        var sht31Temp = measurements.Select(m => m.Sht31?.Temperature ?? string.Empty).Average();
        var sht31Humid = measurements.Select(m => m.Sht31?.Humidity ?? string.Empty).Average();

        return new Measurement
        {
            DeviceId = measurements.First().DeviceId,
            Date = measurements.First().Date.Date,
            Bmp085 = new Bmp085Data
            {
                Temperature = bmp085Temp.ToString(),
                Pressure = (int)bmp085Press,
            },
            Sht31 = new Sht31Data
            {
                Temperature = sht31Temp.ToString(),
                Humidity = sht31Humid.ToString(),
            }
        };
    }

    private static double Average(this IEnumerable<string> values)
    {
        var aggregateResult = values
            .Select(v => double.Parse(v))
            .Where(v => !double.IsNaN(v) && v != 0)
            .Aggregate(
            new { Sum = .0, Count = 0 },
            (acc, number) => new { Sum = acc.Sum + number, Count = acc.Count + 1 }
        );

        double average = (double)aggregateResult.Sum / aggregateResult.Count;

        return Math.Round(average, 2);
    }
}
