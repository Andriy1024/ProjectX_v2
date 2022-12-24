import { NgModule } from '@angular/core';
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

import { DASHBOARD_API_URL, IDENTITY_API_URL } from './app-injection-tokens';
import { environment } from 'src/environments/environment';
import { HttpErrorInterceptor } from './http/http-error.interceptor';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [
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
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: IDENTITY_API_URL, useValue: environment.identityApi },
    { provide: DASHBOARD_API_URL, useValue: environment.dashboardApi },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
