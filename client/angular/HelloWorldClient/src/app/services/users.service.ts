import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserModel } from '../shared/contracts/userModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private usersEndpoint = 'api/users';

  constructor(private http: HttpClient) {}

  getUserById(id: number): Observable<UserModel> {
    return this.http.get<UserModel>(environment.apiPaths.getUserById(id));
  }
}
