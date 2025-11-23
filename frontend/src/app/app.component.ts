import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from './auth/services/auth-service.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent {
  // Modern Angular: Using inject() and signals
  private readonly _authService = inject(AuthService);

  // Convert Observable to Signal - automatically manages subscription
  public readonly isAuthenticated = toSignal(this._authService.isAuthenticated(), { initialValue: false });

  public logOut() {
    this._authService.logOut();
  }

  prepareRoute(outlet: RouterOutlet) {
    if (outlet.isActivated) {
      return outlet.activatedRouteData['tab'];
    }

    return undefined;
  }
}
