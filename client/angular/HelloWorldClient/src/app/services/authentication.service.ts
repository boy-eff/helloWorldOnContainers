import { UsersService } from './users.service';
import { Token } from '../shared/contracts/token';
import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserCredentials } from '../shared/contracts/userCredentials';
import { environment } from 'src/environments/environment';
import { Observable, ReplaySubject, of, switchMap, tap } from 'rxjs';
import { TokenResponse } from '../shared/contracts/tokenResponse';
import jwt_decode from 'jwt-decode';
import { User } from '../shared/contracts/user';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private readonly localStorageTokenKey = 'token';
  private readonly localStorageUserKey = 'user';

  private currentUserSource: ReplaySubject<User | null> =
    new ReplaySubject<User | null>(1);
  currentUser$: Observable<User | null> = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private usersService: UsersService) {
    let user = this.getUser();
    if (user) {
      this.currentUserSource.next(user);
    }
  }

  login(credentials: UserCredentials): Observable<User | null> {
    let body = this._initializeAccessTokenParams(credentials);
    return this.http
      .post<TokenResponse>(environment.apiPaths.tokenEndpoint, body)
      .pipe(
        tap((response) => {
          this._saveTokenInLocalStorage(response);
        }),
        switchMap(() => {
          const userId = this.getToken()?.userId;
          if (userId) {
            return this.usersService.getUserById(userId);
          } else {
            return of(null);
          }
        }),
        tap((user) => {
          if (user) {
            this.currentUserSource.next(user);
            this._saveUserInLocalStorage(user);
          }
        })
      );
  }

  logout() {
    this.currentUserSource.next(null);
    localStorage.removeItem(this.localStorageTokenKey);
    localStorage.removeItem(this.localStorageUserKey);
  }

  refreshAccessToken(): Observable<TokenResponse | null> {
    let token = this.getToken();
    if (token) {
      let params = this._initializeRefreshTokenParams(token.refreshToken);
      return this.http
        .post<TokenResponse>(environment.apiPaths.tokenEndpoint, params)
        .pipe(
          tap((token) => {
            this._saveTokenInLocalStorage(token);
          })
        );
    }
    return of(null);
  }

  getToken(): Token | null {
    let tokenJson = localStorage.getItem(this.localStorageTokenKey);
    if (tokenJson) {
      return JSON.parse(tokenJson);
    } else {
      return null;
    }
  }

  getUser(): User | null {
    let userJson = localStorage.getItem(this.localStorageUserKey);
    if (userJson) {
      return JSON.parse(userJson);
    } else {
      return null;
    }
  }

  private _initializeAccessTokenParams(credentials: UserCredentials) {
    let body = new HttpParams()
      .set('client_id', environment.authentication.clientId)
      .set('client_secret', environment.authentication.clientSecret)
      .set('grant_type', environment.authentication.grantType)
      .set('username', credentials.username)
      .set('password', credentials.password);
    return body;
  }

  private _initializeRefreshTokenParams(refreshToken: string) {
    let body = new HttpParams()
      .set('client_id', environment.authentication.clientId)
      .set('client_secret', environment.authentication.clientSecret)
      .set('grant_type', environment.authentication.refreshGrantType)
      .set('refresh_token', refreshToken);
    return body;
  }

  private _saveTokenInLocalStorage(tokenResponse: TokenResponse) {
    let decodedToken = this._decodeToken(tokenResponse.access_token);
    let token: Token = {
      accessToken: tokenResponse.access_token,
      refreshToken: tokenResponse.refresh_token,
      userId: decodedToken.sub,
    };
    localStorage.setItem(this.localStorageTokenKey, JSON.stringify(token));
  }

  private _saveUserInLocalStorage(user: User) {
    localStorage.setItem(this.localStorageUserKey, JSON.stringify(user));
  }

  private _decodeToken(token: string): any {
    try {
      return jwt_decode(token);
    } catch (Error) {
      return null;
    }
  }
}
