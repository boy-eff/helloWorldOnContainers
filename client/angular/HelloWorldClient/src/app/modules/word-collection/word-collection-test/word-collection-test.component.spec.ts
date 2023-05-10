import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordCollectionTestComponent } from './word-collection-test.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

describe('WordCollectionTestComponent', () => {
  let component: WordCollectionTestComponent;
  let fixture: ComponentFixture<WordCollectionTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WordCollectionTestComponent],
      imports: [HttpClientModule, RouterTestingModule, NgbModule],
    }).compileComponents();

    fixture = TestBed.createComponent(WordCollectionTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
