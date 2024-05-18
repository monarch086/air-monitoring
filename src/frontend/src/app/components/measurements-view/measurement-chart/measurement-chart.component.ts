import { Component, Input, OnInit } from '@angular/core';
import { MeasurementType } from 'src/app/model/measurement-type';

@Component({
  selector: 'app-measurement-chart',
  templateUrl: './measurement-chart.component.html',
  styleUrls: ['./measurement-chart.component.scss']
})
export class MeasurementChartComponent implements OnInit {
  @Input() public type: MeasurementType = MeasurementType.Temperature;
  @Input() public days: number = 1;
  @Input() public data: number[] | null = null;

  public name: string = '';

  public ngOnInit(): void {
    this.name = MeasurementType[this.type];
  }
}
