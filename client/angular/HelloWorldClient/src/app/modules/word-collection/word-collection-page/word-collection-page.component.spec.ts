import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { Word } from 'src/app/shared/contracts/word';
import { WordCollection } from 'src/app/shared/contracts/wordCollection';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { WordsService } from 'src/app/services/words.service';

import { WordCollectionPageComponent } from './word-collection-page.component';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';

describe('WordCollectionPageComponent', () => {
  let component: WordCollectionPageComponent;
  let fixture: ComponentFixture<WordCollectionPageComponent>;
  let wordCollectionsServiceSpy: jasmine.SpyObj<WordCollectionsService>;
  let wordsServiceSpy: jasmine.SpyObj<WordsService>;

  beforeEach(async () => {
    const wordCollectionsService = jasmine.createSpyObj(
      'WordCollectionsService',
      ['getWordCollectionById']
    );
    const wordsService = jasmine.createSpyObj('WordsService', [
      'addWordToDictionary',
    ]);

    await TestBed.configureTestingModule({
      declarations: [WordCollectionPageComponent],
      providers: [
        { provide: WordCollectionsService, useValue: wordCollectionsService },
        { provide: WordsService, useValue: wordsService },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: { get: (paramName: string) => '1' } },
          },
        },
      ],
    }).compileComponents();

    wordCollectionsServiceSpy = TestBed.inject(
      WordCollectionsService
    ) as jasmine.SpyObj<WordCollectionsService>;
    wordsServiceSpy = TestBed.inject(
      WordsService
    ) as jasmine.SpyObj<WordsService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WordCollectionPageComponent);
    component = fixture.componentInstance;
  });

  describe('ngOnInit', () => {
    it('should set the wordCollection property when id is provided', () => {
      const wordCollection: WordCollection = {
        id: 1,
        userId: 1,
        imageUrl: 'test',
        englishLevel: EnglishLevel.Elementary,
        name: 'Collection',
        words: [],
      };
      wordCollectionsServiceSpy.getWordCollectionById.and.returnValue(
        of(wordCollection)
      );

      component.ngOnInit();

      expect(component.wordCollection).toEqual(wordCollection);
    });
  });

  describe('getTranslationsAsString', () => {
    it('should return a string with the translations separated by commas', () => {
      const word: Word = {
        id: 1,
        value: 'Word',
        translations: [
          { id: 1, translation: 'Palabra' },
          { id: 2, translation: 'Word' },
        ],
      };
      const expectedResult = 'Palabra, Word';

      const result = component.getTranslationsAsString(word);

      expect(result).toEqual(expectedResult);
    });
  });
});
