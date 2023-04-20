import { EnglishLevel } from '../enums/EnglishLevel';

export interface RegisterUserModel {
  username: string;
  password: string;
  englishLevel: EnglishLevel;
}
