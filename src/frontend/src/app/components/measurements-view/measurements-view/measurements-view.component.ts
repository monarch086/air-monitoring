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
  public types: typeof MeasurementType = MeasurementType;
  public readonly daysPeriod: number = 1;

  public temperatureData: number[] | null = null;
  public humidityData: number[] | null = null;
  public pressureData: number[] | null = null;

  constructor(private readonly measurementsService: MeasurementsService) {
  }

  public ngOnInit(): void {
    this.measurementsService.getMeasurements(MeasurementType.Temperature, this.daysPeriod)
    .pipe(first())
    .subscribe((response: number[]) => {
      this.temperatureData = response;
    });

    this.measurementsService.getMeasurements(MeasurementType.Humidity, this.daysPeriod)
    .pipe(first())
    .subscribe((response: number[]) => {
      this.humidityData = response;
    });

    this.measurementsService.getMeasurements(MeasurementType.Pressure, this.daysPeriod)
    .pipe(first())
    .subscribe((response: number[]) => {
      this.pressureData = response;
    });
  }
}
