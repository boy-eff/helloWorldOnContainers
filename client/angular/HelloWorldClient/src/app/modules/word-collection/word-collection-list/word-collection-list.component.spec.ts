import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Observable, of } from 'rxjs';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';
import { WordCollection } from 'src/app/shared/contracts/wordCollection';
import { englishLevels } from 'src/app/shared/contracts/englishLevelOption';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { WordCollectionListComponent } from './word-collection-list.component';

class MockWordCollectionsService {
  getWordCollections(
    pageNumber: number,
    pageSize: number,
    englishLevel: EnglishLevel | null
  ): Observable<{ totalCount: number; value: WordCollection[] }> {
    const collections: WordCollection[] = [
      {
        id: 1,
        userId: 1,
        words: [],
        name: 'Collection 1',
        imageUrl: 'https://example.com/image1.png',
        englishLevel: EnglishLevel.Elementary,
      },
      {
        id: 2,
        userId: 1,
        words: [],
        name: 'Collection 2',
        imageUrl: 'https://example.com/image2.png',
        englishLevel: EnglishLevel.Intermediate,
      },
    ];
    return of({ totalCount: collections.length, value: collections });
  }
}

describe('WordCollectionListComponent', () => {
  let component: WordCollectionListComponent;
  let fixture: ComponentFixture<WordCollectionListComponent>;
  let wordCollectionsService: WordCollectionsService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WordCollectionListComponent],
      imports: [RouterTestingModule, FontAwesomeModule],
      providers: [
        {
          provide: WordCollectionsService,
          useClass: MockWordCollectionsService,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WordCollectionListComponent);
    component = fixture.componentInstance;
    wordCollectionsService = TestBed.inject(WordCollectionsService);
    spyOn(wordCollectionsService, 'getWordCollections').and.callThrough();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load collections on init', () => {
    expect(wordCollectionsService.getWordCollections).toHaveBeenCalledWith(
      1,
      component.pageSize,
      null
    );
    expect(component.collections.length).toBe(2);
  });

  it('should load collections with English level filter', () => {
    component.onEnglishLevelChange({
      target: { value: 1 },
    } as unknown as Event);
    expect(wordCollectionsService.getWordCollections).toHaveBeenCalledWith(
      1,
      component.pageSize,
      EnglishLevel.Elementary
    );
    expect(component.collections.length).toBe(2);
  });

  it('should navigate to add collection page', () => {
    const routerSpy = spyOn(component.router, 'navigate');
    component.navigateToAddCollection();
    expect(routerSpy).toHaveBeenCalledWith(['collections', 'add']);
  });
});
