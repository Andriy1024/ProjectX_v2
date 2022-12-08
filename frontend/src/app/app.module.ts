import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { TabsComponent } from './tabs/tabs.component';
import { BookmarksComponent } from './bookmarks/bookmarks.component';
import { TodosComponent } from './todos/todos.component';
import { NotesComponent } from './notes/notes.component';
import { BookmarkTileComponent } from './bookmark-tile/bookmark-tile.component';
import { NoteAddComponent } from './note-add/note-add.component';
import { NoteCardComponent } from './note-card/note-card.component';
import { NoteEditComponent } from './note-edit/note-edit.component';
import { TodoItemComponent } from './todo-item/todo-item.component';
import { TodoAddComponent } from './todo-add/todo-add.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { BookmarkAddComponent } from './bookmark-add/bookmark-add.component';
import { BookmarksManageComponent } from './bookmarks-manage/bookmarks-manage.component';
import { BookmarkEditComponent } from './bookmark-edit/bookmark-edit.component';
import { NotificationComponent } from './notification/notification.component';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';

@NgModule({
  declarations: [
    AppComponent,
    TabsComponent,
    BookmarksComponent,
    TodosComponent,
    NotesComponent,
    BookmarkTileComponent,
    NoteAddComponent,
    NoteCardComponent,
    NoteEditComponent,
    TodoItemComponent,
    TodoAddComponent,
    TodoEditComponent,
    BookmarkAddComponent,
    BookmarksManageComponent,
    BookmarkEditComponent,
    NotificationComponent,
    DynamicFormComponent
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
