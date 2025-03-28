import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MeasurementsViewComponent } from './measurements-view/measurements-view.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MeasurementChartComponent } from './measurement-chart/measurement-chart.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { FormsModule } from '@angular/forms';
import { GetPeriodNamePipe } from './measurement-chart/get-period-name.pipe';

@NgModule({
  declarations: [
    MeasurementsViewComponent,
    MeasurementChartComponent,
    GetPeriodNamePipe
  ],
  imports: [
    FormsModule,
    CommonModule,
    SharedModule,
    NgApexchartsModule
  ]
})
export class MeasurementsViewModule { }
