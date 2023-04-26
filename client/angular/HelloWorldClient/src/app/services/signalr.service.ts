import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from './authentication.service';
import { WordTestQuestion } from '../shared/contracts/wordTestQuestion';
import { ReplaySubject, Observable, Subject } from 'rxjs';

@Injectable()
export class SignalrService {
  private hubConnection: HubConnection;
  private currentWordTestQuestionSource: ReplaySubject<WordTestQuestion | null> =
    new ReplaySubject<WordTestQuestion | null>(1);
  private totalWordsSource: Subject<number> = new Subject<number>();
  currentWordTestQuestion$: Observable<WordTestQuestion | null> =
    this.currentWordTestQuestionSource.asObservable();
  totalWords$: Observable<number> = this.totalWordsSource.asObservable();

  constructor(private authService: AuthenticationService) {}

  public startConnection(wordCollectionId: number) {
    this.currentWordTestQuestionSource = new ReplaySubject();
    this.currentWordTestQuestion$ =
      this.currentWordTestQuestionSource.asObservable();
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(
        environment.apiPaths.wordCollectionTestEndpoint(wordCollectionId),
        {
          headers: {
            Authorization: this.authService.getToken()?.accessToken ?? '',
          },
          accessTokenFactory: () => {
            return this.authService.getToken()?.accessToken ?? '';
          },
        }
      )
      .withAutomaticReconnect([30])
      .build();

    this.hubConnection.start();

    this.onStart();
  }

  public onStart() {
    this.hubConnection.on(
      'Start',
      (question: WordTestQuestion, count: number) => {
        this.currentWordTestQuestionSource.next(question);
        this.totalWordsSource.next(count);
      }
    );
  }

  public async getResultAndCloseConnection() {
    const result = await this.hubConnection.invoke('GetResult');
    await this.hubConnection.stop();
    return result;
  }

  public async getNextWord(userAnswer: string) {
    try {
      const result = await this.hubConnection.invoke(
        'ReceiveAnswerAndSendNextWord',
        userAnswer
      );
      this.currentWordTestQuestionSource.next(result);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}
