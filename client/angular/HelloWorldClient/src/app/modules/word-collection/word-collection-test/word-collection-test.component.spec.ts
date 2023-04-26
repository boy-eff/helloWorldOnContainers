import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionTestComponent } from './word-collection-test.component';

describe('WordCollectionTestComponent', () => {
  let component: WordCollectionTestComponent;
  let fixture: ComponentFixture<WordCollectionTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordCollectionTestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordCollectionTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
