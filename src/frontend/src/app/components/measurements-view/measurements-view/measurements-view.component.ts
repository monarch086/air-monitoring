import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { MeasurementType } from 'src/app/model/measurement-type';
import { MeasurementsService } from 'src/app/services/measurements.service';

@Component({
  selector: 'app-measurements-view',
  templateUrl: './measurements-view.component.html',
  styleUrls: ['./measurements-view.component.scss']
})
export class MeasurementsViewComponent implements OnInit {
  public temperature: number[] = [];

  constructor(private readonly measurementsService: MeasurementsService) {
  }

  public ngOnInit(): void {
    this.measurementsService.getMeasurements(MeasurementType.Temperature, 1).pipe(first()).subscribe((response: number[]) => {
      this.temperature = response;
    });
  }
}
