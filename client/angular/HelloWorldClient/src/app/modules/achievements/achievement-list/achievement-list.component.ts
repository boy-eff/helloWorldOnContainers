import { Component, OnInit } from '@angular/core';
import { AchievementsService } from 'src/app/services/achievements.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Achievement } from 'src/app/shared/contracts/achievement';
import { UsersAchievementsModel } from 'src/app/shared/contracts/usersAchievementsModel';

@Component({
  selector: 'app-achievement-list',
  templateUrl: './achievement-list.component.html',
  styleUrls: ['./achievement-list.component.scss'],
})
export class AchievementListComponent implements OnInit {
  usersAchievements: UsersAchievementsModel[];

  constructor(
    private achievementsService: AchievementsService,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    let userId = this.authService.getUser()?.id;
    if (userId) {
      this.achievementsService
        .getUserAchievements(userId)
        .subscribe((achievements) => (this.usersAchievements = achievements));
    }
  }
}
