import { Component, Input } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-add-word-modal',
  templateUrl: './add-word-modal.component.html',
  styleUrls: ['./add-word-modal.component.scss'],
})
export class AddWordModalComponent {
  addWordForm: FormGroup = new FormGroup({
    word: new FormControl('', [Validators.required]),
    translations: new FormArray([]),
  });

  constructor(public activeModal: NgbActiveModal) {}

  get translationControls(): FormArray {
    return this.addWordForm.get('translations') as FormArray;
  }

  get addWordFormControl() {
    return this.addWordForm.controls;
  }

  addTranslation(): void {
    console.log(this.addWordFormControl?.['word']?.errors?.['required']);
    this.translationControls.push(new FormControl(''));
  }

  removeTranslation(index: number): void {
    this.translationControls.removeAt(index);
  }

  onSubmit(): void {
    this.activeModal.close(this.addWordForm.value);
  }
}
