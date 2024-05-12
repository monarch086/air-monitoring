import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MeasurementsViewComponent } from './components/measurements-view/measurements-view/measurements-view.component';
import { AboutViewComponent } from './components/about/about-view/about-view.component';

const routes: Routes = [
  { path: '', component: MeasurementsViewComponent, loadChildren: () => import('./components/measurements-view/measurements-view.module').then(m => m.MeasurementsViewModule) },
  { path: 'about', component: AboutViewComponent, loadChildren: () => import('./components/about/about.module').then(m => m.AboutModule) },
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
