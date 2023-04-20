import { WordTranslationModel } from './wordTranslationModel';

export interface WordModel {
  id: number;
  value: string;
  translations: WordTranslationModel[];
}
