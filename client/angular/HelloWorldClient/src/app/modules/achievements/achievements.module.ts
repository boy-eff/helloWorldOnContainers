import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AchievementListComponent } from './achievement-list/achievement-list.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [AchievementListComponent],
  imports: [SharedModule],
})
export class AchievementsModule {}
