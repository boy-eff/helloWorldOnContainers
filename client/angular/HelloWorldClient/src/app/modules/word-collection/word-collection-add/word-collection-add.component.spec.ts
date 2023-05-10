import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionAddComponent } from './word-collection-add.component';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';

describe('WordCollectionAddComponent', () => {
  let component: WordCollectionAddComponent;
  let fixture: ComponentFixture<WordCollectionAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WordCollectionAddComponent],
      imports: [SharedModule, HttpClientModule],
    }).compileComponents();

    fixture = TestBed.createComponent(WordCollectionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
