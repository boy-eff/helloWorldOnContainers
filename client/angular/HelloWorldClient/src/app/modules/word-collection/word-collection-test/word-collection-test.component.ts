import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbProgressbarConfig } from '@ng-bootstrap/ng-bootstrap';
import { SignalrService } from 'src/app/services/signalr.service';
import { WordCollectionTestPassInformation } from 'src/app/shared/contracts/wordCollectionTestPassInformation';
import { WordTestQuestion } from 'src/app/shared/contracts/wordTestQuestion';

@Component({
  selector: 'app-word-collection-test',
  templateUrl: './word-collection-test.component.html',
  styleUrls: ['./word-collection-test.component.scss'],
  providers: [SignalrService],
})
export class WordCollectionTestComponent implements OnInit {
  readonly idParamName: string = 'id';
  wordTestQuestion: WordTestQuestion | null;
  currentWordIndex = 0;
  wordsCount = 0;
  testPassResult: WordCollectionTestPassInformation;

  constructor(
    private signalrService: SignalrService,
    private activatedRoute: ActivatedRoute,
    private config: NgbProgressbarConfig
  ) {
    config.striped = true;
    config.animated = true;
    config.type = 'success';
  }
  ngOnInit(): void {
    let id = this.activatedRoute.snapshot.paramMap.get(this.idParamName);
    if (!id) {
      return;
    }

    this.currentWordIndex = 0;

    this.signalrService.startConnection(+id);
    this.signalrService.currentWordTestQuestion$.subscribe(async (value) => {
      if (value === null) {
        let result = await this.signalrService.getResultAndCloseConnection();
        this.testPassResult = result;
      } else {
        this.wordTestQuestion = value;
      }
    });
    this.signalrService.totalWords$.subscribe(
      (count) => (this.wordsCount = count)
    );
  }

  async getNextWord(userAnswer: string) {
    // Get next word and update progress
    await this.signalrService.getNextWord(userAnswer);
    this.currentWordIndex++;
  }
}
