import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UsersAchievements } from '../shared/contracts/usersAchievements';
import { Achievement } from '../shared/contracts/achievement';

@Injectable({
  providedIn: 'root',
})
export class AchievementsService {
  constructor(private http: HttpClient) {}

  getAchievements(): Observable<Achievement[]> {
    return this.http.get<Achievement[]>(
      environment.apiPaths.achievementsEndpoint
    );
  }

  getUserAchievements(userId: number): Observable<UsersAchievements[]> {
    return this.http.get<UsersAchievements[]>(
      environment.apiPaths.usersAchievementsEndpoint(userId)
    );
  }
}
