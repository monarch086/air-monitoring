import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AboutViewComponent } from './about-view/about-view.component';
import { SharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [
    AboutViewComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class AboutModule { }
