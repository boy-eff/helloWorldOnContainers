import { Component } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-word-collection-add',
  templateUrl: './word-collection-add.component.html',
  styleUrls: ['./word-collection-add.component.scss'],
})
export class WordCollectionAddComponent {
  constructor(
    private wordCollectionsService: WordCollectionsService,
    private router: Router
  ) {}

  createCollection(wordCollectionForm: AbstractControl): void {
    let formData =
      this.wordCollectionsService.generateFormData(wordCollectionForm);
    this.wordCollectionsService.createWordCollection(formData).subscribe(() => {
      this.router.navigateByUrl('');
    });
  }
}
