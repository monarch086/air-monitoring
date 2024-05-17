using AirMonitoring.Core.Model;

namespace AirMonitoring.DataProviding.Model
{
    public class QueryModel
    {
        public MeasurementType Type { get; set; }

        public int Days { get; set; }
    }
}
