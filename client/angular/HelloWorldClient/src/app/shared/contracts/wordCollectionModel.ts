import { EnglishLevel } from '../enums/EnglishLevel';

export interface WordCollectionModel {
  id: number;
  name: string;
  englishLevel: EnglishLevel;
  imageUrl: string;
  userId: number;
}
