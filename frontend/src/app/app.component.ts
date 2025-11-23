import { trigger, transition, animate, style, query, group } from '@angular/animations';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from './auth/services/auth-service.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    animations: [
        trigger('routeAmin', [
            transition(':increment', [
                style({
                    position: 'relative',
                    overflow: 'hidden'
                }),
                query(':enter, :leave', [
                    style({
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%'
                    })
                ], { optional: true }),
                group([
                    query(':leave', [
                        animate('200ms ease-in', style({
                            opacity: 0,
                            transform: 'translateX(-100px)'
                        }))
                    ], { optional: true }),
                    query(':enter', [
                        style({
                            transform: 'translateX(100px)',
                            opacity: 0
                        }),
                        animate('250ms 120ms ease-out', style({
                            opacity: 1,
                            transform: 'translateX(0px)'
                        }))
                    ], { optional: true })
                ])
            ]),
            transition(':decrement', [
                style({
                    position: 'relative',
                    overflow: 'hidden'
                }),
                query(':enter, :leave', [
                    style({
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%'
                    })
                ], { optional: true }),
                group([
                    query(':leave', [
                        animate('200ms ease-in', style({
                            opacity: 0,
                            transform: 'translateX(100px)'
                        }))
                    ], { optional: true }),
                    query(':enter', [
                        style({
                            transform: 'translateX(-100px)',
                            opacity: 0
                        }),
                        animate('250ms 120ms ease-out', style({
                            opacity: 1,
                            transform: 'translateX(0px)'
                        }))
                    ], { optional: true })
                ])
            ])
        ])
    ],
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
