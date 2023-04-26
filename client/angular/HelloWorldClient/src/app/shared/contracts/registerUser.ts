import { EnglishLevel } from '../enums/englishLevel';

export interface RegisterUser {
  username: string;
  password: string;
  englishLevel: EnglishLevel;
}
