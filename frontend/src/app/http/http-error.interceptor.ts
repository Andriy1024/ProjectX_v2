import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification/notification.service';
import { IResponse } from '../models/http.models';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(
    //private _authenticationService: AuthService,
      private notificationService: NotificationService)
    {
    }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(this.handleHttpError));
  }

  private handleHttpError = (response: HttpErrorResponse) => {
    console.log(response);
    const result = response?.error as IResponse;
    const message = result?.error?.message || response?.message || "http error";
    this.notificationService.show(message, 5000);

    if ([401, 403].includes(response.status) //&& this._authenticationService.isAuthenticated()
       ) {
        // auto logout if 401 or 403 response returned from api
        //this._authenticationService.logout();
    }
    //return of();
    return throwError(() => response);
  };
}
