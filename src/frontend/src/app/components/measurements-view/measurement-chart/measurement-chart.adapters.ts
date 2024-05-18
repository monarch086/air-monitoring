import { MeasurementItem } from 'src/app/model/measurement-item';
import { AxisLimits, DataSeries, SeriesItem } from './measurement-chart.model';
import { ApexYAxis } from 'ng-apexcharts';
import { MeasurementType } from 'src/app/model/measurement-type';

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

export const metricToAxisConfigAdapter = (type: MeasurementType, limits: AxisLimits): ApexYAxis => {
  return {
    min: limits.min,
    max: limits.max,
    forceNiceScale: false,
    tickAmount: limits.tickAmount,
    title: {
      text: MeasurementType[type],
    },
    opposite: false,
    show: true,
    showAlways: true,
    seriesName: MeasurementType[type]
  } as ApexYAxis;
};
