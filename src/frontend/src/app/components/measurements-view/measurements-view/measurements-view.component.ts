import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { MeasurementItem } from 'src/app/model/measurement-item';
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

  public temperatureData: MeasurementItem[] | null = null;
  public humidityData: MeasurementItem[] | null = null;
  public pressureData: MeasurementItem[] | null = null;

  constructor(private readonly measurementsService: MeasurementsService) {
  }

  public ngOnInit(): void {
    this.measurementsService.getMeasurements(MeasurementType.Temperature, this.daysPeriod)
    .pipe(first())
    .subscribe((response: MeasurementItem[]) => {
      this.temperatureData = response;
    });

    this.measurementsService.getMeasurements(MeasurementType.Humidity, this.daysPeriod)
    .pipe(first())
    .subscribe((response: MeasurementItem[]) => {
      this.humidityData = response;
    });

    this.measurementsService.getMeasurements(MeasurementType.Pressure, this.daysPeriod)
    .pipe(first())
    .subscribe((response: MeasurementItem[]) => {
      this.pressureData = response;
    });
  }
}
