import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionFormComponent } from './word-collection-form.component';

describe('WordCollectionFormComponent', () => {
  let component: WordCollectionFormComponent;
  let fixture: ComponentFixture<WordCollectionFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordCollectionFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordCollectionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
