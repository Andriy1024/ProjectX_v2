import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { DASHBOARD_API_URL } from 'src/app/app-injection-tokens';
import { IDataResponseOf, IResponse, mapResponse, mapResponseOf } from 'src/app/models/http.models';
import { Todo } from 'src/app/models/todo.model';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  constructor(
    private readonly _todoClient: HttpClient,
    @Inject(DASHBOARD_API_URL)
    private readonly _dashboardUrl: string,
    private readonly _notificationService: NotificationService)
    { }

  public getTodos(): Observable<Todo[]> {
    return this._todoClient
      .get<IDataResponseOf<Todo[]>>(`${this._dashboardUrl}/api/tasks`)
      .pipe(
        map(mapResponseOf),
        catchError(e => {
          return of([]);
        })
      );
  }

  public findTodo(id: number): Observable<Todo> {
    return this._todoClient
      .get<IDataResponseOf<Todo>>(`${this._dashboardUrl}/api/tasks/${id}`)
      .pipe(map(mapResponseOf));
  }

  public addTodo(todo: Todo) {
    return this._todoClient
      .post<IDataResponseOf<Todo>>(`${this._dashboardUrl}/api/tasks`, todo)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Task created'))
      );
  }

  public updateTodo(todo: Todo) {
    return this._todoClient
      .put<IDataResponseOf<Todo>>(`${this._dashboardUrl}/api/tasks`, todo)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Task updated'))
      );
  }

  public deleteTodo(id: number) {
    return this._todoClient
      .delete<IResponse>(`${this._dashboardUrl}/api/tasks/${id}`)
      .pipe(
        map(mapResponse),
        tap(r => this._notificationService.show('Task deleted'))
      );
  }
}
