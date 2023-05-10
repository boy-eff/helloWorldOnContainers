import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WordCollection } from '../shared/contracts/wordCollection';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginationResult } from '../shared/contracts/paginationResult';
import { EnglishLevel } from '../shared/enums/englishLevel';
import { AbstractControl } from '@angular/forms';
import { WordCollectionFormValue } from '../shared/contracts/wordCollectionFormValue';

@Injectable({
  providedIn: 'root',
})
export class WordCollectionsService {
  constructor(private http: HttpClient) {}

  getWordCollections(
    pageNumber: number,
    pageSize: number,
    englishLevel: EnglishLevel | null = null
  ): Observable<PaginationResult<WordCollection>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (englishLevel !== null) {
      params = params.set('englishLevel', englishLevel);
    }

    return this.http.get<PaginationResult<WordCollection>>(
      environment.apiPaths.wordCollectionEndpoint,
      {
        params: params,
      }
    );
  }

  getWordCollectionById(collectionId: number): Observable<WordCollection> {
    return this.http.get<WordCollection>(
      environment.apiPaths.wordCollectionWithIdEndpoint(collectionId)
    );
  }

  createWordCollection(wordCollection: FormData): Observable<number> {
    return this.http.post<number>(
      environment.apiPaths.wordCollectionEndpoint,
      wordCollection
    );
  }

  updateWordCollection(
    id: number,
    wordCollection: FormData
  ): Observable<number> {
    return this.http.put<number>(
      environment.apiPaths.wordCollectionWithIdEndpoint(id),
      wordCollection
    );
  }

  deleteWordCollection(id: number): Observable<number> {
    return this.http.delete<number>(
      environment.apiPaths.wordCollectionWithIdEndpoint(id)
    );
  }

  generateFormData(wordCollectionForm: AbstractControl): FormData {
    const formData = new FormData();
    const wordCollectionValue = wordCollectionForm.value;
    formData.append('name', wordCollectionValue.name);
    formData.append('englishLevel', wordCollectionValue.englishLevel);
    formData.append('image', wordCollectionValue.image);
    for (let i = 0; i < wordCollectionValue.words.length; i++) {
      formData.append(`words[${i}].value`, wordCollectionValue.words[i].word);
      for (
        let j = 0;
        j < wordCollectionValue.words[i].translations.length;
        j++
      ) {
        formData.append(
          `words[${i}].translations[${j}]`,
          wordCollectionValue.words[i].translations[j]
        );
      }
    }
    return formData;
  }
}
