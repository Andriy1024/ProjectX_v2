import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { filter, map, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { DASHBOARD_API_URL } from 'src/app/app-injection-tokens';
import { RealtimeMessageTypes } from 'src/app/constants/realtime.const';
import { Bookmark } from 'src/app/models/bookmark.model';
import { IDataResponseOf, IResponse, mapResponse, mapResponseOf } from 'src/app/models/http.models';
import { IRealtimeMessage } from 'src/app/models/realtime.model';
import { NotificationService } from '../notification/notification.service';
import { WebsocketService } from '../websockets/websocket.service';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService {

  constructor(
    private readonly _bookmarkClient: HttpClient,
    @Inject(DASHBOARD_API_URL)
    private  readonly _dashboardUrl: string,
    private readonly _notificationService: NotificationService,
    private readonly _ws: WebsocketService)
    { }

  public getBookmarks(): Observable<Bookmark[]> {
    return this._bookmarkClient
      .get<IDataResponseOf<Bookmark[]>>(`${this._dashboardUrl}/api/bookmarks`)
      .pipe(
        map(mapResponseOf),
        switchMap(bookmarks => this._ws.subscribe()
          .pipe(
            filter(x => RealtimeMessageTypes.isBookmarkMessage(x.type)),
            map(x => x as IRealtimeMessage<Bookmark>),
            map(update => {
              if (update.type == RealtimeMessageTypes.BookmarkUpdated) {
                const bookmarkToUpdate = bookmarks.find(x => x.id === update.message.id);
                if (bookmarkToUpdate) {
                  Object.assign(bookmarkToUpdate, update.message);
                }
              }
              else if (update.type == RealtimeMessageTypes.BookmarkCreated)
                bookmarks.push(update.message);
              else if (update.type == RealtimeMessageTypes.BookmarkDeleted)
                bookmarks = bookmarks.filter(x => x.id !== update.message.id);

              return bookmarks;
            }),
            startWith(bookmarks),
          )
        )
      );
  }

  public findBookmark(id: number) {
    return this._bookmarkClient
      .get<IDataResponseOf<Bookmark>>(`${this._dashboardUrl}/api/bookmarks/${id}`)
      .pipe(map(mapResponseOf));
  }

  public addBookmark(todo: Bookmark) {
    return this._bookmarkClient
      .post<IDataResponseOf<Bookmark>>(`${this._dashboardUrl}/api/bookmarks`, todo)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Bookmark created')),
      );
  }

  public updateBookmark(todo: Bookmark) {
    return this._bookmarkClient
      .put<IDataResponseOf<Bookmark>>(`${this._dashboardUrl}/api/bookmarks`, todo)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Bookmark updated')),
      );
  }

  public deleteBookmark(id: number) {
    return this._bookmarkClient
      .delete<IResponse>(`${this._dashboardUrl}/api/bookmarks/${id}`)
      .pipe(
        map(mapResponse),
        tap(r => this._notificationService.show('Bookmark deleted')),
      );
  }
}
