import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule } from '@angular/forms';
import { AddWordModalComponent } from './add-word-modal.component';

describe('AddWordModalComponent', () => {
  let component: AddWordModalComponent;
  let fixture: ComponentFixture<AddWordModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [AddWordModalComponent],
      providers: [NgbActiveModal],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddWordModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have a form with a word control and a translations form array', () => {
    expect(component.addWordForm.contains('word')).toBe(true);
    expect(component.addWordForm.contains('translations')).toBe(true);
  });

  it('should require the word control to be filled', () => {
    const wordControl = component.addWordForm.get('word')!;
    wordControl.setValue('');
    expect(wordControl.valid).toBe(false);
    expect(wordControl.errors?.['required']).toBe(true);

    wordControl.setValue('test');
    expect(wordControl.valid).toBe(true);
  });

  it('should require at least one translation control to be filled', () => {
    const translationControl = component.translationControls.controls[0];
    translationControl.setValue('');
    expect(translationControl.valid).toBe(false);
    expect(translationControl.errors?.['required']).toBe(true);

    translationControl.setValue('test');
    expect(translationControl.valid).toBe(true);
  });

  it('should be able to add and remove translations', () => {
    component.addTranslation();
    expect(component.translationControls.length).toBe(2);

    component.removeTranslation(0);
    expect(component.translationControls.length).toBe(1);
  });
});
