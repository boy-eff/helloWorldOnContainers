import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WordCollection } from '../shared/contracts/wordCollection';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginationResult } from '../shared/contracts/paginationResult';

@Injectable({
  providedIn: 'root',
})
export class WordCollectionsService {
  constructor(private http: HttpClient) {}

  getWordCollections(
    pageNumber: number,
    pageSize: number
  ): Observable<PaginationResult<WordCollection>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<PaginationResult<WordCollection>>(
      environment.apiPaths.wordCollectionEndpoint,
      {
        params: params,
      }
    );
  }

  getWordCollectionById(collectionId: number): Observable<WordCollection> {
    return this.http.get<WordCollection>(
      environment.apiPaths.getWordCollectionById(collectionId)
    );
  }

  createWordCollection(wordCollection: any): Observable<number> {
    return this.http.post<number>(
      environment.apiPaths.wordCollectionEndpoint,
      wordCollection
    );
  }
}
