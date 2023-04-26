import { PassedWordTestQuestion } from './passedWordTestQuestion';

export interface WordCollectionTestPassInformation {
  id: number;
  userId: number;
  wordCollectionId: number;
  questions: PassedWordTestQuestion[];
  totalQuestions: number;
  correctAnswersAmount: number;
}
