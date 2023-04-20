import { UsersService } from './users.service';
import { TokenModel } from './../shared/contracts/tokenModel';
import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserCredentials } from '../shared/contracts/userCredentials';
import { environment } from 'src/environments/environment';
import { Observable, ReplaySubject, of, switchMap, tap } from 'rxjs';
import { TokenResponse } from '../shared/contracts/tokenResponse';
import jwt_decode from 'jwt-decode';
import { UserModel } from '../shared/contracts/userModel';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private tokenEndpoint = 'connect/token';
  private readonly localStorageTokenKey = 'token';
  private readonly localStorageUserKey = 'user';
  private currentUserSource: ReplaySubject<UserModel | null> =
    new ReplaySubject<UserModel | null>(1);
  currentUser$: Observable<UserModel | null> =
    this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private usersService: UsersService) {
    let user = this.getUser();
    if (user) {
      this.currentUserSource.next(user);
    }
  }

  login(credentials: UserCredentials): Observable<UserModel | null> {
    let body = this._initializeBody(credentials);
    return this.http
      .post<TokenResponse>(
        environment.apiPaths.identity + this.tokenEndpoint,
        body
      )
      .pipe(
        tap((response) => {
          this._setToken(response);
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
            this._setUser(user);
          }
        })
      );
  }

  logout() {
    this.currentUserSource.next(null);
    localStorage.removeItem(this.localStorageTokenKey);
  }

  getToken(): TokenModel | null {
    let tokenJson = localStorage.getItem(this.localStorageTokenKey);
    if (tokenJson) {
      return JSON.parse(tokenJson);
    } else {
      return null;
    }
  }

  getUser(): UserModel | null {
    let userJson = localStorage.getItem(this.localStorageUserKey);
    if (userJson) {
      return JSON.parse(userJson);
    } else {
      return null;
    }
  }

  private _initializeBody(credentials: UserCredentials) {
    let body = new HttpParams()
      .set('client_id', environment.authentication.client_id)
      .set('client_secret', environment.authentication.client_secret)
      .set('grant_type', environment.authentication.grant_type)
      .set('username', credentials.username)
      .set('password', credentials.password);
    return body;
  }

  private _setToken(tokenResponse: TokenResponse) {
    let decodedToken = this._decodeToken(tokenResponse.access_token);
    let token: TokenModel = {
      accessToken: tokenResponse.access_token,
      refreshToken: tokenResponse.refresh_token,
      userId: decodedToken.sub,
    };
    localStorage.setItem(this.localStorageTokenKey, JSON.stringify(token));
  }

  private _setUser(user: UserModel) {
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
