import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, filter, map, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { DASHBOARD_API_URL } from 'src/app/app-injection-tokens';
import { RealtimeMessageTypes } from 'src/app/constants/realtime.const';
import { IDataResponseOf, IResponse, mapResponseOf, mapResponse } from 'src/app/models/http.models';
import { Note } from 'src/app/models/note.model';
import { IRealtimeMessage } from 'src/app/models/realtime.model';
import { NotificationService } from '../notification/notification.service';
import { WebsocketService } from '../websockets/websocket.service';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  constructor(
    private readonly _noteClient: HttpClient,
    @Inject(DASHBOARD_API_URL)
    private readonly _dashboardUrl: string,
    private readonly _notificationService: NotificationService,
    private readonly _ws: WebsocketService)
    { }

  public getNotes(): Observable<Note[]> {
    return this._noteClient
      .get<IDataResponseOf<Note[]>>(`${this._dashboardUrl}/api/notes`)
      .pipe(
        map(mapResponseOf),
        switchMap((notes: Note[]) => this._ws.subscribe()
          .pipe(
            filter(x => RealtimeMessageTypes.isNoteMessage(x.type)),
            map(x => x as IRealtimeMessage<Note>),
            map((update: IRealtimeMessage<Note>) => {
              if (update.type == RealtimeMessageTypes.NoteUpdated) {
                const noteToUpdate = notes.find(x => x.id === update.message.id);
                if (noteToUpdate) {
                  Object.assign(noteToUpdate, update.message);
                }
              }
              else if (update.type == RealtimeMessageTypes.NoteCreated)
                notes.push(update.message);
              else if (update.type == RealtimeMessageTypes.NoteDeleted)
                notes = notes.filter(x => x.id !== update.message.id);

              return notes;
            }),
            startWith(notes)
          )
        ),
        catchError(e => {
          return of([]);
        })
      );
  }

  public findNote(id: string): Observable<Note> {
    return this._noteClient
        .get<IDataResponseOf<Note>>(`${this._dashboardUrl}/api/notes/${id}`)
        .pipe(
          map(mapResponseOf),
          // switchMap((note: Note) => {
          //   return this._ws.subscribe()
          //   .pipe(
          //     filter(x => x.type === 'NoteUpdated' && x.message.id == id),
          //     map(x => x.message as Note),
          //     startWith(note)
          //   )
          // })
        );
  }

  public addNote(note: Note): Observable<Note> {
    return this._noteClient
      .post<IDataResponseOf<Note>>(`${this._dashboardUrl}/api/notes`, note)
      .pipe(
        map(mapResponseOf),
        tap((s) => this._notificationService.show('Note created'))
      );
  }

  public updateNote(note: Note) {
    return this._noteClient
      .put<IDataResponseOf<Note>>(`${this._dashboardUrl}/api/notes`, note)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Note updated'))
      );
  }

  public deleteNote(id: number): Observable<IResponse> {
    return this._noteClient
      .delete<IResponse>(`${this._dashboardUrl}/api/notes/${id}`)
      .pipe(
        map(mapResponse),
        tap(r => this._notificationService.show('Note deleted'))
      );
  }
}
