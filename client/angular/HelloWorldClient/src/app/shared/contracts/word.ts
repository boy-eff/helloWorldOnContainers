import { WordTranslation } from './wordTranslation';

export interface Word {
  id: number;
  value: string;
  translations: WordTranslation[];
}
