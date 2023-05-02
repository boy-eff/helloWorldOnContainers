import { CustomEnum } from '../contracts/customEnum';

export enum EnglishLevel {
  Elementary = 0,
  PreIntermediate = 1,
  Intermediate = 2,
  UpperIntermediate = 3,
  Advanced = 4,
  Proficient = 5,
}

export function englishLevelAsCustomEnum(): CustomEnum[] {
  return Object.keys(EnglishLevel)
    .filter((k) => parseInt(k) >= 0)
    .map((k) => ({ value: Number(k), label: EnglishLevel[Number(k)] }));
}
