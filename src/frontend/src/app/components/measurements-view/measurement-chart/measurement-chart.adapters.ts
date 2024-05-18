import { MeasurementItem } from 'src/app/model/measurement-item';
import { DataSeries, SeriesItem } from './measurement-chart.model';

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
