import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionAddComponent } from './word-collection-add.component';

describe('WordCollectionAddComponent', () => {
  let component: WordCollectionAddComponent;
  let fixture: ComponentFixture<WordCollectionAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordCollectionAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordCollectionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
