import { EnglishLevel } from '../enums/englishLevel';

export interface User {
  id: number;
  englishLevel: EnglishLevel;
  userName: string;
  imageUrl: string;
}
