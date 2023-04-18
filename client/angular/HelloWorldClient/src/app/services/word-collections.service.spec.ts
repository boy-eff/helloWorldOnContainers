import { TestBed } from '@angular/core/testing';

import { WordCollectionsService } from './word-collections.service';

describe('WordCollectionsService', () => {
  let service: WordCollectionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WordCollectionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
