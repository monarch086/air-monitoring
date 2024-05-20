using AirMonitoring.Core.Model.Anomaly;
using AirMonitoring.Core.Model.MeasurementModel;
using System.Text;

namespace AirMonitoring.DataAnalysis
{
    internal static class AnomalyMessageGenerator
    {
        public static string Generate(AnomalyType anomaly, Measurement measurement)
        {
            var sb = new StringBuilder("Anomaly detected: ");

            switch (anomaly)
            {
                case AnomalyType.OverCool:
                    sb.Append($"too low temperature - {measurement.Bmp085.Temperature}°C.");
                    break;

                case AnomalyType.OverHeat:
                    sb.Append($"too high temperature - {measurement.Bmp085.Temperature}°C.");
                    break;

                case AnomalyType.OverDry:
                    sb.Append($"too low humidity - {measurement.Sht31.Humidity}%.");
                    break;

                case AnomalyType.SuddenWarming:
                case AnomalyType.SuddenCooling:
                    sb.Append($"quick temperature change to {measurement.Bmp085.Temperature}°C.");
                    break;

                default:
                    break;
            }

            return sb.ToString();
        }
    }
}
