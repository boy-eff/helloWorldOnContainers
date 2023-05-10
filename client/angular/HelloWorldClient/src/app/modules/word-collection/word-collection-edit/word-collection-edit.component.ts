import { WordCollection } from './../../../shared/contracts/wordCollection';
import { Component, OnInit } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { WordCollectionsService } from 'src/app/services/word-collections.service';

@Component({
  selector: 'app-word-collection-edit',
  templateUrl: './word-collection-edit.component.html',
  styleUrls: ['./word-collection-edit.component.scss'],
})
export class WordCollectionEditComponent implements OnInit {
  readonly idParamName: string = 'id';
  wordCollection$: Observable<WordCollection>;
  wordCollectionId: number;

  constructor(
    private wordCollectionsService: WordCollectionsService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.wordCollectionId = Number(
      this.activatedRoute.snapshot.paramMap.get(this.idParamName)
    );
    if (!this.wordCollectionId) {
      return;
    }

    this.wordCollection$ = this.wordCollectionsService.getWordCollectionById(
      Number(this.wordCollectionId)
    );
  }

  updateCollection(wordCollectionForm: AbstractControl): void {
    let formData =
      this.wordCollectionsService.generateFormData(wordCollectionForm);
    this.wordCollectionsService
      .updateWordCollection(this.wordCollectionId, formData)
      .subscribe(() => {
        this.router.navigateByUrl('');
      });
  }
}
