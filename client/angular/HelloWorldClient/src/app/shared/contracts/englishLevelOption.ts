import { EnglishLevel } from '../enums/englishLevel';

export interface EnglishLevelOption {
  value: number;
  displayValue: string;
}

export const englishLevels = [
  { value: EnglishLevel.Elementary, displayValue: 'Elementary' },
  { value: EnglishLevel.PreIntermediate, displayValue: 'Pre-Intermediate' },
  { value: EnglishLevel.Intermediate, displayValue: 'Intermediate' },
  { value: EnglishLevel.UpperIntermediate, displayValue: 'Upper-Intermediate' },
  { value: EnglishLevel.Advanced, displayValue: 'Advanced' },
  { value: EnglishLevel.Proficient, displayValue: 'Proficient' },
];
