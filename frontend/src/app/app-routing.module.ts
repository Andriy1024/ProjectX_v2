import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';

import { NotesComponent } from './components/notes/notes.component';

import { TodosComponent } from './todos/todos.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';

import { BookmarksComponent } from './bookmarks/bookmarks.component';
import { BookmarkAddComponent } from './bookmark-add/bookmark-add.component';
import { BookmarksManageComponent } from './bookmarks-manage/bookmarks-manage.component';
import { BookmarkEditComponent } from './bookmark-edit/bookmark-edit.component';

const routes: Routes = [
  { path: '', redirectTo: '/bookmarks', pathMatch: 'full' },
  { path: 'bookmarks', component: BookmarksComponent, data: { tab: 1 } },
  { path: 'bookmarks/add', component: BookmarkAddComponent },
  { path: 'bookmarks/manage', component: BookmarksManageComponent, children: [
    { path: ':id', component: BookmarkEditComponent }
  ] },
  { path: 'todos', component: TodosComponent, data: { tab: 2 } },
  { path: 'todos/:id', component: TodoEditComponent },
  { path: 'notes', component: NotesComponent, data: { tab: 3 } },
  { path: 'form', component: DynamicFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
