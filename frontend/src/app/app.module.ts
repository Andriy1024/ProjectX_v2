import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { TabsComponent } from './tabs/tabs.component';
import { NotificationComponent } from './notification/notification.component';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';

import { NotesComponent } from './notes/notes.component';

import { TodosComponent } from './todos/todos.component';
import { TodoItemComponent } from './todo-item/todo-item.component';
import { TodoAddComponent } from './todo-add/todo-add.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';

import { BookmarksComponent } from './bookmarks/bookmarks.component';
import { BookmarkAddComponent } from './bookmark-add/bookmark-add.component';
import { BookmarksManageComponent } from './bookmarks-manage/bookmarks-manage.component';
import { BookmarkEditComponent } from './bookmark-edit/bookmark-edit.component';
import { BookmarkTileComponent } from './bookmark-tile/bookmark-tile.component';

@NgModule({
  declarations: [
    AppComponent,
    TabsComponent,
    NotificationComponent,
    DynamicFormComponent,

    BookmarksComponent,
    BookmarkAddComponent,
    BookmarksManageComponent,
    BookmarkEditComponent,
    BookmarkTileComponent,

    TodosComponent,
    TodoItemComponent,
    TodoAddComponent,
    TodoEditComponent,

    NotesComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
