import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { Component, OnInit } from '@angular/core';
import { WordCollection } from 'src/app/shared/contracts/wordCollection';
import { ActivatedRoute } from '@angular/router';
import { WordsService } from 'src/app/services/words.service';
import { Word } from 'src/app/shared/contracts/word';

@Component({
  selector: 'app-word-collection-page',
  templateUrl: './word-collection-page.component.html',
  styleUrls: ['./word-collection-page.component.scss'],
})
export class WordCollectionPageComponent implements OnInit {
  readonly idParamName: string = 'id';
  wordCollection: WordCollection | null;

  constructor(
    private wordCollectionsService: WordCollectionsService,
    private wordsService: WordsService,
    public activateRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    let id = this.activateRoute.snapshot.paramMap.get(this.idParamName);
    if (!id) {
      return;
    }

    this.wordCollectionsService
      .getWordCollectionById(+id)
      .subscribe((response) => (this.wordCollection = response));
  }

  getTranslationsAsString(word: Word) {
    return word.translations.map((x) => x.translation).join(', ');
  }

  addWordToDictionary(id: number) {
    this.wordsService
      .addWordToDictionary(id)
      .subscribe((result) => console.log(result));
  }
}
