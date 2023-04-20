import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionPageComponent } from './word-collection-page.component';

describe('WordCollectionPageComponent', () => {
  let component: WordCollectionPageComponent;
  let fixture: ComponentFixture<WordCollectionPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordCollectionPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordCollectionPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
