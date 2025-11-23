import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './services/auth-service.service';

export const authGuard: CanActivateFn = (route, state): Observable<boolean | UrlTree> => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.isAuthenticated().pipe(
    map(isAuthenticated => {
      if (isAuthenticated) {
        return true;
      }
      
      // Store the attempted URL for redirecting after login
      authService.logOut(state.url);
      
      // Redirect to sign-in page
      return router.createUrlTree(['/sign-in']);
    })
  );
};
