import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { NotificationService } from '../services/notification/notification.service';
import { IResponse } from '../models/http.models';
import { AuthService } from '../auth/services/auth-service.service';
import { LoggerService } from '../services/logging/logger.service';

@Injectable()
export class ApplicationHttpInterceptor implements HttpInterceptor {

  constructor(
      private _authenticationService: AuthService,
      private notificationService: NotificationService,
      private logger: LoggerService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(this.addAuthHeader(request))
      .pipe(
        catchError(err => {
          return err.status == 401
            ? this.refreshToken(request, next, err)
            : this.handleHttpError(err)
        })
      )
  }

  private handleHttpError = (response: HttpErrorResponse) => {
    this.logger.error('HTTP Error', response);
    const result = response?.error as IResponse;
    const message = result?.error?.message || response?.message || "http error";
    this.notificationService.show(message, 5000);
    return throwError(() => response);
  };

  private refreshToken = (request: HttpRequest<any>, next: HttpHandler, response: HttpErrorResponse) => {
    return this._authenticationService
        .refreshToken()
        .pipe(
          switchMap((isRefreshed: boolean) => {
             if (isRefreshed) {
              return next.handle(this.addAuthHeader(request))
                .pipe(
                  catchError(err => {
                    return this.handleHttpError(err);
                  })
                )
            }
            return throwError(() => response);
          })
        );
  }

  private addAuthHeader = (request: HttpRequest<any>) => {
    const token = this._authenticationService.getToken();

    if (token) {
      return request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + token.token)
      });
    }

    return request;
  }
}
