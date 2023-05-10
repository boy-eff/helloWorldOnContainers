import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { faEdit, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {
  EnglishLevelOption,
  englishLevels,
} from 'src/app/shared/contracts/englishLevelOption';
import { minCount } from 'src/app/shared/validators/minCount.validator';
import { AddWordModalComponent } from '../add-word-modal/add-word-modal.component';
import { WordCollection } from 'src/app/shared/contracts/wordCollection';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';

@Component({
  selector: 'app-word-collection-form',
  templateUrl: './word-collection-form.component.html',
  styleUrls: ['./word-collection-form.component.scss'],
})
export class WordCollectionFormComponent implements OnInit {
  faPlus = faPlus;
  faTrash = faTrash;
  faEdit = faEdit;
  englishLevels: EnglishLevelOption[] = englishLevels;
  @Output() submitEvent = new EventEmitter<AbstractControl>();
  @Input() wordCollection: WordCollection = {
    id: 0,
    name: '',
    englishLevel: EnglishLevel.Elementary,
    imageUrl: '',
    userId: 0,
    words: [],
  };

  wordCollectionForm: FormGroup;

  get words(): FormArray {
    return this.wordCollectionForm.get('words') as FormArray;
  }

  constructor(private modalService: NgbModal) {}
  ngOnInit(): void {
    console.log(this.wordCollection);
    this.wordCollectionForm = new FormGroup({
      name: new FormControl(this.wordCollection.name, [Validators.required]),
      englishLevel: new FormControl(
        EnglishLevel[this.wordCollection.englishLevel],
        [Validators.required]
      ),
      imageName: new FormControl(''),
      image: new FormControl(''),
      words: new FormArray([], [minCount(3)]),
    });
    for (const word of this.wordCollection.words) {
      this.words.push(
        this.createWordFormGroup(
          word.value,
          word.translations.map((x) => x.translation)
        )
      );
    }
  }

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

  openAddWordModal(): void {
    const modalRef = this.modalService.open(AddWordModalComponent);
    modalRef.result.then(
      (result) => {
        this.words.push(
          this.createWordFormGroup(result.word, result.translations)
        );
      },
      (reason) => {}
    );
  }

  openEditWordModal(i: any) {
    const modalRef = this.modalService.open(AddWordModalComponent);
    modalRef.componentInstance.addWordForm = this.words.controls[i];
  }

  onSubmit() {
    this.submitEvent.emit(this.wordCollectionForm);
  }

  private createWordFormGroup(value: string, translations: any[]): FormGroup {
    return new FormGroup({
      word: new FormControl(value),
      translations: new FormArray(translations.map((x) => new FormControl(x))),
    });
  }
}
