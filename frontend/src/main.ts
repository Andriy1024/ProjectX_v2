import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { AppComponent } from './app/app.component';
import { AppRoutingModule } from './app/app-routing.module';
import { DynamicFormModule } from './app/components/dynamic-form/dynamic-form.module';
import { applicationHttpInterceptor } from './app/http/application-http.interceptor.fn';
import { DASHBOARD_API_URL, IDENTITY_API_URL, REALTIME_API_URL } from './app/app-injection-tokens';
import { environment } from './environments/environment';
import { PROJECT_X_SESSION } from './app/auth/auth.const';

function tokenGetter(): string | null {
  const stringToken = localStorage.getItem(PROJECT_X_SESSION);
  if (stringToken) {
    try {
      const parsed = JSON.parse(stringToken);
      return parsed?.token ?? null;
    } catch {
      return null;
    }
  }
  return null;
}

bootstrapApplication(AppComponent, {
  providers: [
    // Bring in existing NgModules as providers
    importProvidersFrom(
      AppRoutingModule,
      DynamicFormModule,
      JwtModule.forRoot({
        config: {
          tokenGetter,
          allowedDomains: environment.tokenWhiteListedDomains,
          disallowedRoutes: ['']
        }
      })
    ),

    // App-wide DI tokens
    { provide: IDENTITY_API_URL, useValue: environment.identityApi },
    { provide: DASHBOARD_API_URL, useValue: environment.dashboardApi },
    { provide: REALTIME_API_URL, useValue: environment.realtimeApi },

    // Http client + interceptors
    provideHttpClient(
      withInterceptors([applicationHttpInterceptor]),
      withInterceptorsFromDi()
    )
  ]
}).catch(err => console.error(err));
