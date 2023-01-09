import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from "@angular/router";
import { map, Observable } from "rxjs";
import { AuthService } from "./services/auth-service.service";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(private authenticationService: AuthService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
      return this.authenticationService.isAuthenticated()
        .pipe(
          map((isAuthenticated) => {
            if (!isAuthenticated) {
              this.authenticationService.logOut(state.url);
            }

            return isAuthenticated;
          })
        );
    }
}
