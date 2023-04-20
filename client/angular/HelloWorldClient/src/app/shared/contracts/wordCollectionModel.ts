import { EnglishLevel } from '../enums/EnglishLevel';
import { WordModel } from './wordModel';

export interface WordCollectionModel {
  id: number;
  name: string;
  englishLevel: EnglishLevel;
  imageUrl: string;
  userId: number;
  words: WordModel[];
}
