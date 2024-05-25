import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MeasurementsViewComponent } from './components/measurements-view/measurements-view/measurements-view.component';

const routes: Routes = [
  { path: '', component: MeasurementsViewComponent, loadChildren: () => import('./components/measurements-view/measurements-view.module').then(m => m.MeasurementsViewModule) },
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      (routes),
      { bindToComponentInputs: true }
    )
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
