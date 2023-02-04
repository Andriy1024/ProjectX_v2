import { trigger, transition, animate, style, query, group } from '@angular/animations';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
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
              opacity: 0 }),
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
              opacity: 0 }),
            animate('250ms 120ms ease-out', style({
              opacity: 1,
              transform: 'translateX(0px)'
            }))
          ], { optional: true })
        ])
      ])
    ])
  ]
})
export class AppComponent {

  public isAuthenticated = false;

  constructor(private readonly _authService: AuthService) {

  }

  ngOnInit() {
    this._authService.isAuthenticated().subscribe(r => {
      this.isAuthenticated = r;
    });
  }

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
