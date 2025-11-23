import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../auth/services/auth-service.service';
import { IResponse } from '../models/http.models';
import { NotificationService } from '../services/notification/notification.service';
import { LoggerService } from '../services/logging/logger.service';

export const applicationHttpInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const notificationService = inject(NotificationService);
  const logger = inject(LoggerService);

  const addAuthHeader = (req: typeof request) => {
    const token = authService.getToken();
    if (token) {
      return req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + token.token)
      });
    }
    return req;
  };

  const handleHttpError = (response: HttpErrorResponse) => {
    logger.error('HTTP Error', response);
    const result = response?.error as IResponse;
    const message = result?.error?.message || response?.message || "http error";
    notificationService.show(message, 5000);
    return throwError(() => response);
  };

  const refreshToken = (req: typeof request, response: HttpErrorResponse) => {
    return authService.refreshToken().pipe(
      switchMap((isRefreshed: boolean) => {
        if (isRefreshed) {
          return next(addAuthHeader(req)).pipe(
            catchError(err => handleHttpError(err))
          );
        }
        return throwError(() => response);
      })
    );
  };

  return next(request).pipe(
    catchError(err => {
      return err.status === 401
        ? refreshToken(request, err)
        : handleHttpError(err);
    })
  );
};
