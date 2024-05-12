import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../model/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private readonly httpClient: HttpClient) {}

  public getUsers(): Observable<User[]> {
    return this.httpClient.get<User[]>(`${environment.APIGateway}/${environment.envName}/users`);
  }
}
