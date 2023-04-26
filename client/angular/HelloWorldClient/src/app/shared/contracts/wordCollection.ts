import { EnglishLevel } from '../enums/englishLevel';
import { Word } from './word';

export interface WordCollection {
  id: number;
  name: string;
  englishLevel: EnglishLevel;
  imageUrl: string;
  userId: number;
  words: Word[];
}
