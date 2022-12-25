import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of, tap, throwError } from 'rxjs';
import { DASHBOARD_API_URL } from 'src/app/app-injection-tokens';
import { IDataResponseOf, IResponse, mapResponseOf, mapResponse } from 'src/app/models/http.models';
import { Note } from 'src/app/models/note.model';
import { NotificationData } from 'src/app/models/notification-data.model';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  constructor(
    private readonly _noteClient: HttpClient,
    @Inject(DASHBOARD_API_URL)
    private  readonly _dashboardUrl: string,
    private readonly _notificationService: NotificationService)
    { }

  public getNotes(): Observable<Note[]> {
    return this._noteClient
      .get<IDataResponseOf<Note[]>>(`${this._dashboardUrl}/api/notes`)
      .pipe(
        map(mapResponseOf),
        catchError(e => {
          return of([]);
        })
      );
  }

  public findNote(id: string): Observable<Note> {
    return this._noteClient
        .get<IDataResponseOf<Note>>(`${this._dashboardUrl}/api/notes/${id}`)
        .pipe(map(mapResponseOf));
  }

  public addNote(note: Note): Observable<Note> {
    return this._noteClient
      .post<IDataResponseOf<Note>>(`${this._dashboardUrl}/api/notes`, note)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Note created'))
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
