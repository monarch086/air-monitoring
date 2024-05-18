import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, switchMap } from 'rxjs';
import { MeasurementItem } from 'src/app/model/measurement-item';
import { MeasurementType } from 'src/app/model/measurement-type';
import { PeriodSelectorItem } from 'src/app/model/period-selector-item';
import { MeasurementsService } from 'src/app/services/measurements.service';
import { PERIOD_ITEMS } from './measurement-view.constants';

@Component({
  selector: 'app-measurements-view',
  templateUrl: './measurements-view.component.html',
  styleUrls: ['./measurements-view.component.scss']
})
export class MeasurementsViewComponent implements OnInit {
  public types: typeof MeasurementType = MeasurementType;
  public readonly periods: PeriodSelectorItem[] = PERIOD_ITEMS;
  public selectedPeriod: number = 1;

  public temperatureData: MeasurementItem[] | null = null;
  public humidityData: MeasurementItem[] | null = null;
  public pressureData: MeasurementItem[] | null = null;

  private loadTempStream$: BehaviorSubject<number> = new BehaviorSubject<number>(this.selectedPeriod);
  private loadHumidStream$: BehaviorSubject<number> = new BehaviorSubject<number>(this.selectedPeriod);
  private loadPressStream$: BehaviorSubject<number> = new BehaviorSubject<number>(this.selectedPeriod);

  constructor(private readonly measurementsService: MeasurementsService) {
  }

  public ngOnInit(): void {
    this.loadTempStream$.pipe(
      switchMap(period => this.measurementsService.getMeasurements(MeasurementType.Temperature, period))
    ).subscribe((response: MeasurementItem[]) => {
      this.temperatureData = response;
    });

    this.loadHumidStream$.pipe(
      switchMap(period => this.measurementsService.getMeasurements(MeasurementType.Humidity, period))
    ).subscribe((response: MeasurementItem[]) => {
      this.humidityData = response;
    });

    this.loadPressStream$.pipe(
      switchMap(period => this.measurementsService.getMeasurements(MeasurementType.Pressure, period))
    ).subscribe((response: MeasurementItem[]) => {
      this.pressureData = response;
    });
  }

  public handlePeriodUpdated(period: number): void {
    this.clearData();

    this.loadTempStream$.next(period);
    this.loadHumidStream$.next(period);
    this.loadPressStream$.next(period);
  }

  private clearData(): void {
    this.temperatureData = null;
    this.humidityData = null;
    this.pressureData = null;
  }
}
