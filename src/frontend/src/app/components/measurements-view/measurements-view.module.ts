import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MeasurementsViewComponent } from './measurements-view/measurements-view.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    MeasurementsViewComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class MeasurementsViewModule { }
