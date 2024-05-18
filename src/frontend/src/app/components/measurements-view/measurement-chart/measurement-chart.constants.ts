import { ChartOptions } from './measurement-chart.model';

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
