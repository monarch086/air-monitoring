import { MeasurementType } from 'src/app/model/measurement-type';
import { AxisLimits, ChartOptions } from './measurement-chart.model';

export const CHART_COLORS: string[] = [
  '--vz-info',
  '--vz-primary',
  '--vz-danger',
  '--vz-warning',
  '--vz-success'
];

const TOOLBAR_Y_POSITION: number = -37;

export const DEFAULT_CHART_OPTIONS: ChartOptions = {
  series: [],
  chart: {
    type: 'line',
    stacked: false,
    height: '232px',
    zoom: {
      type: 'x',
      enabled: true
    },
    toolbar: {
      autoSelected: 'zoom',
      offsetY: TOOLBAR_Y_POSITION
    },
  },
  colors: [],
  dataLabels: {
    enabled: false,
  },
  stroke: {
    width: [3, 3],
    curve: 'straight'
  },
  markers: {
    size: 6,
  },
  yaxis: [
    {
      min: 0,
      max: 40,
      tickAmount: 4
    }
  ],
  xaxis: {
    type: 'datetime',
  },
  grid: {
    show: true,
    xaxis: {
      lines: {
          show: true
      }
    }
  },
  legend: {
    show: true,
    position: 'top'
  },
};

export const MEASUREMENT_LIMITS: Map<MeasurementType, AxisLimits> = new Map([
  [ MeasurementType.Temperature, { min: 0, max: 40, tickAmount: 4 } as AxisLimits ],
  [ MeasurementType.Humidity, { min: 0, max: 60, tickAmount: 4 } as AxisLimits ],
  [ MeasurementType.Pressure, { min: 50000, max: 150000, tickAmount: 4 } as AxisLimits ],
]);

export const DEFAULT_LIMITS: AxisLimits = {
  min: 0,
  max: 40,
  tickAmount: 4
} as AxisLimits;
