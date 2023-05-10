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
    translations: new FormArray([new FormControl('', Validators.required)]),
  });

  constructor(public activeModal: NgbActiveModal) {}

  get translationControls(): FormArray {
    return this.addWordForm.get('translations') as FormArray;
  }

  addTranslation(): void {
    this.translationControls.push(new FormControl('', [Validators.required]));
  }

  removeTranslation(index: number): void {
    this.translationControls.removeAt(index);
  }

  onSubmit(): void {
    this.activeModal.close(this.addWordForm.value);
  }
}
