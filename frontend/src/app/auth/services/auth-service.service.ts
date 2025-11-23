import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of, shareReplay, Subject, tap } from 'rxjs';
import { IDENTITY_API_URL } from 'src/app/app-injection-tokens';
import { AuthRequest, AuthResponse } from '../auth.models';
import { IDataResponseOf, mapResponseOf } from 'src/app/models/http.models';
import { PROJECT_X_SESSION } from '../auth.const';
import { Token } from '../auth.models';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { LoggerService } from 'src/app/services/logging/logger.service';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private refreshingAccessToken: boolean = false;
  private readonly $accessTokenRefreshed: Subject<boolean> = new Subject<boolean>();

  private readonly _http = inject(HttpClient);
  private readonly authUrl = inject(IDENTITY_API_URL);
  private readonly _jwtHelper = inject(JwtHelperService);
  private readonly _router = inject(Router);
  private readonly _logger = inject(LoggerService);

  public logIn(request: AuthRequest): Observable<AuthResponse> {
    return this._http
      .post<IDataResponseOf<AuthResponse>>(`${this.authUrl}/api/auth/sign-in`,
        new HttpParams({
          fromObject: {
            email: request.email,
            password: request.password
        }}))
      .pipe(
        //shareReplay(), //Avoiding duplicate HTTP requests
        map(mapResponseOf),
        tap(response => {
          this.saveSession({
            token: response.token,
            refreshToken: response.refreshToken
          })
        })
      );
  }

  public logOut(returnUrl: string | undefined = undefined): void {
    this.removeSession();
    this._router.navigate(['/sign-in'], { queryParams: { returnUrl: returnUrl } });
  }

  public refreshToken(): Observable<boolean> {
    const token = this.getToken();

    if (!token) {
      return of(false);
    }

    return this.tryRefreshToken(token);
  }

  public isAuthenticated(): Observable<boolean> {
      const token = this.getToken();

      if (!token) return of(false);

      const isTokenExpired = this._jwtHelper.isTokenExpired(token.token);

      if (isTokenExpired) {
        return this.tryRefreshToken(token);
      }

      return of(!isTokenExpired);
  }

  private tryRefreshToken(token: Token): Observable<boolean> {

    if (this.refreshingAccessToken) {
      return new Observable<boolean>(observer => {
        this.$accessTokenRefreshed.subscribe((result) => {
          // this code will run when the access token has been refreshed
          observer.next(result);
          observer.complete();
        })
      })
    }

    this.refreshingAccessToken = true;

    return this._http
      .post<IDataResponseOf<AuthResponse>>(`${this.authUrl}/api/auth/refresh-token`,
        new HttpParams({
          fromObject: {
            token: token.token,
            refreshToken: token.refreshToken
        }}))
      .pipe(
        map(mapResponseOf),
        map(response => {
          this.saveSession({
            token: response.token,
            refreshToken: response.refreshToken
          });

          this._logger.info('Token refreshed');
          return true;
        }),
        catchError(err => {
          this._logger.error('Failed to refresh token', err);
          this.removeSession();
          return of(false);
        }),
        tap(result => {
          this.refreshingAccessToken = false;
          this.$accessTokenRefreshed.next(result);
        })
      );
  }

  public getToken(): Token | null {
    const stringToken = this.getStringTokenObject();

    if (stringToken) {
        const parsed = JSON.parse(stringToken);

        return {
          token: parsed.token,
          refreshToken: parsed.refreshToken
        };
    }

    return null;
  }

  private getStringTokenObject(): string | null {
    return localStorage.getItem(PROJECT_X_SESSION);
  }

  private saveSession(token: Token): void {
    localStorage.setItem(PROJECT_X_SESSION, JSON.stringify(token));
  }

  private removeSession(): void {
    localStorage.removeItem(PROJECT_X_SESSION);
  }
}
