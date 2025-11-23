import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { AppComponent } from './app.component';
import { TabsComponent } from './tabs/tabs.component';
import { NotificationComponent } from './notification/notification.component';
import { DynamicFormModule } from './components/dynamic-form/dynamic-form.module';

import { DASHBOARD_API_URL, IDENTITY_API_URL, REALTIME_API_URL } from './app-injection-tokens';
import { environment } from 'src/environments/environment';
import { applicationHttpInterceptor } from './http/application-http.interceptor.fn';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { PROJECT_X_SESSION } from './auth/auth.const';

@NgModule({ declarations: [
        AppComponent,
        TabsComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        CommonModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        NotificationComponent,
        DynamicFormModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                allowedDomains: environment.tokenWhiteListedDomains,
                disallowedRoutes: [''],
            },
        })], providers: [
        { provide: IDENTITY_API_URL, useValue: environment.identityApi },
        { provide: DASHBOARD_API_URL, useValue: environment.dashboardApi },
        { provide: REALTIME_API_URL, useValue: environment.realtimeApi },
        provideHttpClient(withInterceptors([applicationHttpInterceptor])),
        provideAnimationsAsync()
    ] })
export class AppModule { }

export function tokenGetter()
{
      const stringToken = localStorage.getItem(PROJECT_X_SESSION);

      if(stringToken)
      {
          const parsed = JSON.parse(stringToken);
          return parsed?.token;
      }

      return null;
}
