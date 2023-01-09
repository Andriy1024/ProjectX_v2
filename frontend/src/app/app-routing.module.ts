import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';

import { NotesComponent } from './components/notes/notes.component';

import { TodosComponent } from './components/todos/todos.component';

import { BookmarksComponent } from './bookmarks/bookmarks.component';
import { BookmarksManageComponent } from './bookmarks-manage/bookmarks-manage.component';
import { BookmarkEditComponent } from './bookmark-edit/bookmark-edit.component';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/bookmarks', pathMatch: 'full' },
  { path: 'bookmarks', component: BookmarksComponent, canActivate: [AuthGuard], data: { tab: 1 } },
  { path: 'bookmarks/manage', component: BookmarksManageComponent, canActivate: [AuthGuard], children: [
    { path: ':id', component: BookmarkEditComponent }
  ] },
  { path: 'todos', component: TodosComponent, canActivate: [AuthGuard], data: { tab: 2 } },
  { path: 'notes', component: NotesComponent, data: { tab: 3 } },
  { path: 'form', component: DynamicFormComponent, canActivate: [AuthGuard] },
  { path: 'sign-in', component: SignInComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
