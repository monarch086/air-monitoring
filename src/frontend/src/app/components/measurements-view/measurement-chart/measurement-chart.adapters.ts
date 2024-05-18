import { MeasurementItem } from 'src/app/model/measurement-item';
import { AxisLimits, DataSeries, SeriesItem } from './measurement-chart.model';
import { ApexYAxis } from 'ng-apexcharts';

export const measurementItemsToSeries = (data: MeasurementItem[], name: string): DataSeries => {
  const series = ({
    name: name,
    data: data.map(item =>
      ({
        x: item.Date.getTime(),
        y: item.Value
      } as SeriesItem))
  } as DataSeries);

  return series;
}

export const metricToAxisConfigAdapter = (limits: AxisLimits): ApexYAxis => {
  return {
    min: limits.min,
    max: limits.max,
    forceNiceScale: false,
    tickAmount: limits.tickAmount,
    title: {
      text: limits.title,
    },
    opposite: false,
    show: true,
    showAlways: true,
    seriesName: limits.title
  } as ApexYAxis;
};
