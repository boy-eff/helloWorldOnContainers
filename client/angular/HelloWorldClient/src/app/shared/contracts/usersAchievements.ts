import { Achievement } from './achievement';
import { AchievementLevel } from './achievementLevel';

export interface UsersAchievements {
  userId: number;
  achievement: Achievement;
  pointsAchieved: number;
  nextLevel: AchievementLevel;
  achieveDate: Date;
}
