import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MeasurementsViewComponent } from './measurements-view/measurements-view.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MeasurementChartComponent } from './measurement-chart/measurement-chart.component';
import { NgApexchartsModule } from 'ng-apexcharts';

@NgModule({
  declarations: [
    MeasurementsViewComponent,
    MeasurementChartComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    NgApexchartsModule
  ]
})
export class MeasurementsViewModule { }
