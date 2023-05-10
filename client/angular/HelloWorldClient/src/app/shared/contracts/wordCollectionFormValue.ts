import { EnglishLevel } from '../enums/englishLevel';
import { Word } from './word';

export interface WordCollectionFormValue {
  name: string;
  englishLevel: EnglishLevel;
  image: File;
  words: Word[];
}
