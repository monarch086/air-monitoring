import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { MeasurementType } from 'src/app/model/measurement-type';
import { ChartOptions } from './measurement-chart.model';
import { CHART_COLORS, DEFAULT_CHART_OPTIONS } from './measurement-chart.constants';
import { MeasurementItem } from 'src/app/model/measurement-item';
import { measurementItemsToSeries } from './measurement-chart.adapters';

@Component({
  selector: 'app-measurement-chart',
  templateUrl: './measurement-chart.component.html',
  styleUrls: ['./measurement-chart.component.scss']
})
export class MeasurementChartComponent implements OnInit, OnChanges {
  @Input() public type: MeasurementType = MeasurementType.Temperature;
  @Input() public days: number = 1;
  @Input() public data: MeasurementItem[] | null = null;

  public name: string = '';
  public chartOptions: ChartOptions = DEFAULT_CHART_OPTIONS;

  public ngOnInit(): void {
    this.name = MeasurementType[this.type];
  }

  public ngOnChanges(): void {
    this.chartOptions = this.createChartOptions();
  }

  public createChartOptions(): ChartOptions {
    const options = {
      ...DEFAULT_CHART_OPTIONS,
      series: [ measurementItemsToSeries(this.data ?? [], this.name) ],
      colors: this.getChartColorsArray(CHART_COLORS),
    } as ChartOptions;

    return options;
  }

  private getChartColorsArray(colors: string[]): unknown {
    return colors.map(function (value: any) {
      const newValue = value.replace(' ', '');
      if (newValue.indexOf(',') === -1) {
        let color = getComputedStyle(document.documentElement).getPropertyValue(
          newValue
        );
        if (color) {
          color = color.replace(' ', '');
          return color;
        } else return newValue;
      } else {
        const val = value.split(',');
        if (val.length == 2) {
          let rgbaColor = getComputedStyle(
            document.documentElement
          ).getPropertyValue(val[0]);
          rgbaColor = 'rgba(' + rgbaColor + ',' + val[1] + ')';
          return rgbaColor;
        } else {
          return newValue;
        }
      }
    });
  }
}
