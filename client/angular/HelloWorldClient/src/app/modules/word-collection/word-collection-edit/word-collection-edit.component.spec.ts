import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionEditComponent } from './word-collection-edit.component';

describe('WordCollectionEditComponent', () => {
  let component: WordCollectionEditComponent;
  let fixture: ComponentFixture<WordCollectionEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordCollectionEditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordCollectionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
