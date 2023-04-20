import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WordCollectionModel } from '../shared/contracts/wordCollectionModel';
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
  ): Observable<PaginationResult<WordCollectionModel>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<PaginationResult<WordCollectionModel>>(
      environment.apiPaths.getWordCollections(),
      {
        params: params,
      }
    );
  }

  getWordCollectionById(collectionId: number): Observable<WordCollectionModel> {
    return this.http.get<WordCollectionModel>(
      environment.apiPaths.getWordCollectionById(collectionId)
    );
  }
}
