import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddWordModalComponent } from './add-word-modal.component';

describe('AddWordModalComponent', () => {
  let component: AddWordModalComponent;
  let fixture: ComponentFixture<AddWordModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddWordModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddWordModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
