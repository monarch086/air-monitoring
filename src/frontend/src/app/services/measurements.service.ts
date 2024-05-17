import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MeasurementType } from '../model/measurement-type';

@Injectable({
  providedIn: 'root'
})
export class MeasurementsService {

  constructor(private readonly httpClient: HttpClient) {}

  public getMeasurements(type: MeasurementType, days: number): Observable<number[]> {
    const params = new HttpParams()
      .set('Type', type)
      .set('Days', days);

    return this.httpClient.get<string[]>(`${environment.APIGateway}/${environment.envName}/measurements`, { params })
    .pipe(map((response: string[]) => response.map(i => Number(i))));
  }
}
