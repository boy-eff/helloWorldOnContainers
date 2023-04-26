import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { faEdit, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { Word } from 'src/app/shared/contracts/word';
import { englishLevelAsArray } from 'src/app/shared/enums/englishLevel';
import { AddWordModalComponent } from '../add-word-modal/add-word-modal.component';

@Component({
  selector: 'app-word-collection-add',
  templateUrl: './word-collection-add.component.html',
  styleUrls: ['./word-collection-add.component.scss'],
})
export class WordCollectionAddComponent {
  faPlus = faPlus;
  faTrash = faTrash;
  faEdit = faEdit;
  englishLevels = englishLevelAsArray();
  word: Word = {
    id: 0,
    value: '',
    translations: [{ id: 0, translation: '' }],
  };
  wordCollectionForm: FormGroup = new FormGroup({
    name: new FormControl(''),
    englishLevel: new FormControl('', [Validators.required]),
    imageName: new FormControl(''),
    image: new FormControl(''),
    words: new FormArray([]),
  });

  addWordForm: FormGroup = new FormGroup({
    value: new FormControl(''),
    translations: new FormControl(''),
  });

  get words(): FormArray {
    return this.wordCollectionForm.get('words') as FormArray;
  }

  constructor(
    private wordCollectionsService: WordCollectionsService,
    private modalService: NgbModal
  ) {}

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.wordCollectionForm.patchValue({
        image: file,
      });
    }
  }

  removeWord(index: number): void {
    this.words.removeAt(index);
  }

  createCollection() {
    console.log(this.wordCollectionForm.value);
    const formData = new FormData();
    const value = this.wordCollectionForm.value;
    formData.append('name', value.name);
    formData.append('englishLevel', value.englishLevel);
    formData.append('image', value.image);
    for (let i = 0; i < value.words.length; i++) {
      formData.append(`words[${i}].value`, value.words[i].value);
      for (let j = 0; j < value.words[i].translations.length; j++) {
        formData.append(
          `words[${i}].translations[${j}]`,
          value.words[i].translations[j]
        );
      }
    }
    this.wordCollectionsService
      .createWordCollection(formData)
      .subscribe((value) => console.log(value));
  }

  openAddWordModal(): void {
    const modalRef = this.modalService.open(AddWordModalComponent);
    modalRef.result.then(
      (result) => {
        console.log(result);
        this.words.push(
          this.createWordFormGroup(result.value, result.translations)
        );
      },
      (reason) => {}
    );
  }

  openEditWordModal(i: any) {
    const modalRef = this.modalService.open(AddWordModalComponent);
    modalRef.componentInstance.addWordForm = this.words.controls[i];
  }

  private createWordFormGroup(value: string, translations: any[]): FormGroup {
    return new FormGroup({
      value: new FormControl(value),
      translations: new FormArray(translations.map((x) => new FormControl(x))),
    });
  }
}
