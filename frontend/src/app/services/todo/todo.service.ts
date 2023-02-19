import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { filter, map, Observable, startWith, switchMap, tap } from 'rxjs';
import { DASHBOARD_API_URL } from 'src/app/app-injection-tokens';
import { RealtimeMessageTypes } from 'src/app/constants/realtime.const';
import { IDataResponseOf, IResponse, mapResponse, mapResponseOf } from 'src/app/models/http.models';
import { IRealtimeMessage } from 'src/app/models/realtime.model';
import { Todo } from 'src/app/models/todo.model';
import { NotificationService } from '../notification/notification.service';
import { WebsocketService } from '../websockets/websocket.service';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  constructor(
    private readonly _todoClient: HttpClient,
    @Inject(DASHBOARD_API_URL)
    private readonly _dashboardUrl: string,
    private readonly _notificationService: NotificationService,
    private readonly _ws: WebsocketService)
    { }

  public getTodos(): Observable<Todo[]> {
    return this._todoClient
      .get<IDataResponseOf<Todo[]>>(`${this._dashboardUrl}/api/tasks`)
      .pipe(
        map(mapResponseOf),
        switchMap((todos: Todo[]) => this._ws.subscribe()
          .pipe(
            filter(x => RealtimeMessageTypes.isTaskMessage(x.type)),
            map(x => x as IRealtimeMessage<Todo>),
            map(update => {
              if (update.type == RealtimeMessageTypes.TaskUpdated) {
                const todoToUpdate = todos.find(x => x.id === update.message.id);
                if (todoToUpdate) {
                  Object.assign(todoToUpdate, update.message);
                }
              }
              else if (update.type == RealtimeMessageTypes.TaskCreated)
                todos.push(update.message);
              else if (update.type == RealtimeMessageTypes.TaskDeleted)
                todos = todos.filter(x => x.id !== update.message.id);

              return todos;
            }),
            startWith(todos),
          )
        )
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
