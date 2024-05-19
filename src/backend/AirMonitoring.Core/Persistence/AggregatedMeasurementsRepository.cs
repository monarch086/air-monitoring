using Amazon.Lambda.Core;

namespace AirMonitoring.Core.Persistence
{
    public class AggregatedMeasurementsRepository : MeasurementsRepo
    {
        protected override string TableName => "AirMonitoring.MeasurementsAggregated";

        public AggregatedMeasurementsRepository(ILambdaLogger logger) : base(logger)
        {
        }
    }
}
