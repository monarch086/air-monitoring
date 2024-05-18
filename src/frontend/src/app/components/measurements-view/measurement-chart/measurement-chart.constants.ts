import { MeasurementType } from 'src/app/model/measurement-type';
import { AxisLimits, ChartOptions } from './measurement-chart.model';
import { ApexYAxis } from 'ng-apexcharts';

export const CHART_COLORS: string[] = [
  '#2E93fA', '#66DA26', '#546E7A', '#E91E63', '#FF9800'
];

const TOOLBAR_Y_POSITION: number = -37;

export const DEFAULT_LIMITS: AxisLimits = {
  min: 0,
  max: 40,
  tickAmount: 4,
  title: 'N/A'
} as AxisLimits;

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
    width: 2,
    curve: 'smooth'
  },
  yaxis: [
    DEFAULT_LIMITS as ApexYAxis
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
  [ MeasurementType.Temperature, { min: 0, max: 40, tickAmount: 4, title: 'Temperature, °C' } as AxisLimits ],
  [ MeasurementType.Humidity, { min: 0, max: 60, tickAmount: 4, title: 'Humidity, %' } as AxisLimits ],
  [ MeasurementType.Pressure, { min: 99000, max: 100000, tickAmount: 4, title: 'Pressure, Pa' } as AxisLimits ],
]);
