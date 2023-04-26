import { EnumArray } from '../contracts/enumArray';

export enum EnglishLevel {
  Elementary,
  PreIntermediate,
  Intermediate,
  UpperIntermediate,
  Advanced,
  Proficient,
}

export function englishLevelAsArray(): EnumArray[] {
  return Object.keys(EnglishLevel)
    .filter((k) => parseInt(k) >= 0)
    .map((k) => ({ value: Number(k), label: EnglishLevel[Number(k)] }));
}
