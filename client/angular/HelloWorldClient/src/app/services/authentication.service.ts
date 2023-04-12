import { TokenModel } from './../shared/contracts/tokenModel';
import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserCredentials } from '../shared/contracts/userCredentials';
import { environment } from 'src/environments/environment';
import { Observable, ReplaySubject, map } from 'rxjs';
import { TokenResponse } from '../shared/contracts/tokenResponse';
import { Router } from '@angular/router';
import jwt_decode from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService implements OnInit {

  private tokenEndpoint = "connect/token";
  private registerEndpoint = "api/users";
  private localStorageTokenKey = "token";
  private currentTokenSource = new ReplaySubject<TokenModel | null>(1);
  currentToken$ = this.currentTokenSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this._loadToken();
  }

  login(credentials: UserCredentials): Observable<TokenResponse> {
    let body = this._initializeBody(credentials);
    return this.http.post<TokenResponse>(environment.apiPaths.identity + this.tokenEndpoint, body)
      .pipe(
        map((response) => {
          this._setToken(response);
          return response;
        })
      )
  }

  register(credentials: UserCredentials){
    return this.http.post(environment.apiPaths.identity + this.registerEndpoint, credentials);
  }

  logout(){
    localStorage.removeItem(this.localStorageTokenKey);
    this.currentTokenSource.next(null);
  }

  private _loadToken() {
    let token = localStorage.getItem(this.localStorageTokenKey);
    if (token) {
      const tokenModel = JSON.parse(token);
      this.currentTokenSource.next(tokenModel);
    }
  }

  private _initializeBody(credentials: UserCredentials) {
    let body = new HttpParams()
      .set("client_id", environment.authentication.client_id)
      .set("client_secret", environment.authentication.client_secret)
      .set("grant_type", environment.authentication.grant_type)
      .set("username", credentials.username)
      .set("password", credentials.password)
    return body;
  }

  private _setToken(tokenResponse: TokenResponse) {
    let decodedToken = this._decodeToken(tokenResponse.accessToken);
    let token: TokenModel = { accessToken: tokenResponse.accessToken, refreshToken: tokenResponse.refreshToken, userId: 1 }

    localStorage.setItem(this.localStorageTokenKey, JSON.stringify(token));
    this.currentTokenSource.next(token);
  }

  private _decodeToken(token: string): any {
    try {
      return jwt_decode(token);
    } catch(Error) {
      return null;
    };
  }
}
