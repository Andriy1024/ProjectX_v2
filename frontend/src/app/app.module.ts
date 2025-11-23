import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { TabsComponent } from './tabs/tabs.component';
import { NotificationComponent } from './notification/notification.component';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';
import { NotesComponent } from './components/notes/notes.component';
import { TodosComponent } from './components/todos/todos.component';

import { BookmarksComponent } from './bookmarks/bookmarks.component';
import { BookmarksManageComponent } from './bookmarks-manage/bookmarks-manage.component';
import { BookmarkEditComponent } from './bookmark-edit/bookmark-edit.component';
import { BookmarkTileComponent } from './bookmark-tile/bookmark-tile.component';

import { DASHBOARD_API_URL, IDENTITY_API_URL, REALTIME_API_URL } from './app-injection-tokens';
import { environment } from 'src/environments/environment';
import { ApplicationHttpInterceptor } from './http/application-http.interceptor';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { JwtModule } from '@auth0/angular-jwt';

@NgModule({ declarations: [
        AppComponent,
        TabsComponent,
        NotificationComponent,
        DynamicFormComponent,
        TodosComponent,
        NotesComponent,
        BookmarksComponent,
        BookmarksManageComponent,
        BookmarkEditComponent,
        BookmarkTileComponent,
        SignInComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        CommonModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
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
        { provide: HTTP_INTERCEPTORS, useClass: ApplicationHttpInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }

export function tokenGetter()
{
      // const stringToken = localStorage.getItem(ACCESS_TOKEN_KEY);

      // if(stringToken != null)
      // {
      //     const parsed = JSON.parse(stringToken);
      //     return parsed?.access_token;
      // }

      return null;
}
