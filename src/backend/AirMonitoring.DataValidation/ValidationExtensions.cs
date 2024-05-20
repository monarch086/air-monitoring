using AirMonitoring.Core.Model.MeasurementModel;

namespace AirMonitoring.DataValidation
{
    internal static class ValidationExtensions
    {
        public static bool Validate(this Measurement measurement)
        {
            try
            {
                if (string.IsNullOrEmpty(measurement.DeviceId) ||
                    measurement.Date > DateTime.UtcNow ||
                    measurement.Date < DateTime.Parse("2023-01-01") ||
                    float.Parse(measurement.Bmp085?.Temperature) < ValidationLimits.MIN_TEMP ||
                    float.Parse(measurement.Bmp085?.Temperature) > ValidationLimits.MAX_TEMP ||
                    float.Parse(measurement.Sht31?.Humidity) < ValidationLimits.MIN_HUMID ||
                    float.Parse(measurement.Sht31?.Humidity) > ValidationLimits.MAX_HUMID ||
                    measurement.Bmp085?.Pressure < ValidationLimits.MIN_PRESS ||
                    measurement.Bmp085?.Pressure > ValidationLimits.MAX_PRESS)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
