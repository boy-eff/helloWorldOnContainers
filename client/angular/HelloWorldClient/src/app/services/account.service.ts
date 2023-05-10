import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  changePassword(oldPassword: string, newPassword: string): Observable<number> {
    const body = { oldPassword: oldPassword, newPassword: newPassword };
    return this.http.post<number>(
      environment.apiPaths.changePasswordEndpoint,
      body
    );
  }

  changeImage(image: File) {
    const formData = new FormData();
    formData.append('file', image);
    return this.http.post(
      environment.apiPaths.updateUserImageEndpoint,
      formData
    );
  }
}
