using AirMonitoring.Core.Model.Anomaly;
using AirMonitoring.Core.Model.MeasurementModel;

namespace AirMonitoring.DataAnalysis
{
    internal static class AnomalyExtensions
    {
        public static AnomalyType? HasAnomaly(this Measurement measurement, Measurement previousAggregatedMeasurement)
        {
            var tempDifference = Math.Abs(float.Parse(measurement.Bmp085?.Temperature) - float.Parse(previousAggregatedMeasurement.Bmp085?.Temperature));
            var isCooling = float.Parse(measurement.Bmp085?.Temperature) < float.Parse(previousAggregatedMeasurement.Bmp085?.Temperature);

            if (float.Parse(measurement.Bmp085?.Temperature) < AnomalyLimits.MIN_TEMP ||
                float.Parse(measurement.Sht31?.Temperature) < AnomalyLimits.MIN_TEMP)
            {
                return AnomalyType.OverCool;
            }
            else if (float.Parse(measurement.Bmp085?.Temperature) > AnomalyLimits.MAX_TEMP ||
                     float.Parse(measurement.Sht31?.Temperature) > AnomalyLimits.MAX_TEMP)
            {
                return AnomalyType.OverHeat;
            }
            else if (float.Parse(measurement.Sht31?.Humidity) < AnomalyLimits.MIN_HUMID)
            {
                return AnomalyType.OverDry;
            }
            else if (tempDifference > AnomalyLimits.TEMP_CHANGE_DIFF && isCooling)
            {
                return AnomalyType.SuddenCooling;
            }
            else if (tempDifference > AnomalyLimits.TEMP_CHANGE_DIFF && !isCooling)
            {
                return AnomalyType.SuddenWarming;
            }

            return null;
        }
    }
}
