import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NoteAddComponent } from './note-add/note-add.component';
import { BookmarksComponent } from './bookmarks/bookmarks.component';
import { NotesComponent } from './notes/notes.component';
import { TodosComponent } from './todos/todos.component';
import { NoteEditComponent } from './note-edit/note-edit.component';
import { TodoAddComponent } from './todo-add/todo-add.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { BookmarkAddComponent } from './bookmark-add/bookmark-add.component';
import { BookmarksManageComponent } from './bookmarks-manage/bookmarks-manage.component';
import { BookmarkEditComponent } from './bookmark-edit/bookmark-edit.component';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/bookmarks', pathMatch: 'full' },
  { path: 'bookmarks', component: BookmarksComponent, data: { tab: 1 } },
  { path: 'bookmarks/add', component: BookmarkAddComponent },
  { path: 'bookmarks/manage', component: BookmarksManageComponent, children: [
    { path: ':id', component: BookmarkEditComponent }
  ] },
  { path: 'todos', component: TodosComponent, data: { tab: 2 } },
  { path: 'todos/add', component: TodoAddComponent },
  { path: 'todos/:id', component: TodoEditComponent },
  { path: 'notes', component: NotesComponent, data: { tab: 3 } },
  { path: 'notes/add', component: NoteAddComponent },
  { path: 'notes/:id', component: NoteEditComponent },

  { path: 'form', component: DynamicFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
