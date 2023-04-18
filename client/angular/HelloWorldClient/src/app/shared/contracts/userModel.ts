import { EnglishLevel } from '../enums/EnglishLevel';

export interface UserModel {
  id: number;
  englishLevel: EnglishLevel;
  username: string;
  imageUrl: string;
}
