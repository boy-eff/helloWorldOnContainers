import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class WordsService {
  constructor(private http: HttpClient) {}

  addWordToDictionary(wordId: number): Observable<number> {
    return this.http.post<number>(
      environment.apiPaths.addWordToDictionaryEndpoint(wordId),
      null
    );
  }
}
