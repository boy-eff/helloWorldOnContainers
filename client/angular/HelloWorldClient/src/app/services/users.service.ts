import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../shared/contracts/user';
import { Observable } from 'rxjs';
import { RegisterUser } from '../shared/contracts/registerUser';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private http: HttpClient) {}

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(environment.apiPaths.wordsUserEndpoint(id));
  }

  registerUser(user: RegisterUser): Observable<User> {
    return this.http.post<User>(
      environment.apiPaths.identityUsersEndpoint,
      user
    );
  }
}
