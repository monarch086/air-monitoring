using AirMonitoring.Core.Model.MeasurementModel;

namespace AirMonitoring.DataProviding.Model
{
    public class QueryModel
    {
        public MeasurementType Type { get; set; }

        public int Days { get; set; }
    }
}
