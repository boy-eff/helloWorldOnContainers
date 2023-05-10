import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpStatusCode,
} from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { TokenResponse } from '../contracts/tokenResponse';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private readonly authService: AuthenticationService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const tokenModel = this.authService.getToken();
    if (!tokenModel) {
      return next.handle(request);
    }

    const newRequest = this.addAuthorizationHeader(
      request,
      tokenModel.accessToken
    );
    return next.handle(newRequest).pipe(
      catchError((err) => {
        if (
          err instanceof HttpErrorResponse &&
          err.status === HttpStatusCode.Unauthorized &&
          tokenModel
        ) {
          return this.refreshAccessToken(request, next);
        }
        return throwError(() => err);
      })
    );
  }

  private refreshAccessToken(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<any> {
    return this.authService.refreshAccessToken().pipe(
      switchMap((token: TokenResponse | null) => {
        if (token) {
          const newRequest = this.addAuthorizationHeader(
            request,
            token.access_token
          );
          return next.handle(newRequest);
        }
        return of(null);
      }),
      catchError((error) => {
        if (
          error instanceof HttpErrorResponse &&
          error.status == HttpStatusCode.Unauthorized
        ) {
          this.authService.logout();
        }

        return throwError(() => error);
      })
    );
  }

  private addAuthorizationHeader(
    request: HttpRequest<any>,
    accessToken: string
  ): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }
}
