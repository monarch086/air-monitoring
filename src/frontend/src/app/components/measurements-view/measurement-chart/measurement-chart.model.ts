import {
  ApexChart,
  ApexDataLabels,
  ApexGrid,
  ApexLegend,
  ApexStroke,
  ApexTooltip,
  ApexXAxis,
  ApexYAxis,
} from 'ng-apexcharts';

export type ChartOptions = {
  series: DataSeries[];
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  stroke: ApexStroke;
  colors: any[];
  xaxis: ApexXAxis;
  yaxis: ApexYAxis | ApexYAxis[];
  grid: ApexGrid;
  tooltip?: ApexTooltip;
  legend: ApexLegend;
};

export interface DataSeries {
  name: string;
  data: SeriesItem[];
}

export interface SeriesItem {
  x: number;
  y: number;
}

export interface AxisLimits {
  min: number;
  max: number;
  tickAmount: number;
  title: string;
}
