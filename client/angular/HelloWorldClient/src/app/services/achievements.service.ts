import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UsersAchievements } from '../shared/contracts/usersAchievements';

@Injectable({
  providedIn: 'root',
})
export class AchievementsService {
  constructor(private http: HttpClient) {}

  getUserAchievements(userId: number): Observable<UsersAchievements[]> {
    return this.http.get<UsersAchievements[]>(
      environment.apiPaths.achievementsEndpoint(userId)
    );
  }
}
