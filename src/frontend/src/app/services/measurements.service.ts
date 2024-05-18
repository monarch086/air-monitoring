import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MeasurementType } from '../model/measurement-type';
import { MeasurementItem, MeasurementResponseItem } from '../model/measurement-item';

@Injectable({
  providedIn: 'root'
})
export class MeasurementsService {

  constructor(private readonly httpClient: HttpClient) {}

  public getMeasurements(type: MeasurementType, days: number): Observable<MeasurementItem[]> {
    const params = new HttpParams()
      .set('Type', type)
      .set('Days', days);

    return this.httpClient.get<MeasurementResponseItem[]>(`${environment.APIGateway}/${environment.envName}/measurements`, { params })
    .pipe(map((response: MeasurementResponseItem[]) => response.map(i =>
      ({
        Date: new Date(i.Date),
        Value: Number(i.Value)
      } as MeasurementItem))));
  }
}
