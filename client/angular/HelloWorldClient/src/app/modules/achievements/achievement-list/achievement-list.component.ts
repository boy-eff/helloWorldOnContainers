import { Component, OnInit } from '@angular/core';
import { AchievementsService } from 'src/app/services/achievements.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UsersAchievements } from 'src/app/shared/contracts/usersAchievements';

@Component({
  selector: 'app-achievement-list',
  templateUrl: './achievement-list.component.html',
  styleUrls: ['./achievement-list.component.scss'],
})
export class AchievementListComponent implements OnInit {
  usersAchievements: UsersAchievements[];

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
