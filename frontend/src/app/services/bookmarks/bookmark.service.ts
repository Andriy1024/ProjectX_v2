import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of, Subscription, switchMap, tap } from 'rxjs';
import { DASHBOARD_API_URL } from 'src/app/app-injection-tokens';
import { Bookmark } from 'src/app/models/bookmark.model';
import { IDataResponseOf, IResponse, mapResponse, mapResponseOf } from 'src/app/models/http.models';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService {

  // private bookmarks: Bookmark[] = [
  //   new Bookmark('Google', 'https://www.google.com/'),
  //   new Bookmark('YouTube', 'https://www.youtube.com/'),
  //   new Bookmark('Twitter', 'https://www.twitter.com/')
  // ];

  private bookmarkUpdates$ = new BehaviorSubject<Bookmark[]>([]);

  constructor(
    private readonly _bookmarkClient: HttpClient,
    @Inject(DASHBOARD_API_URL)
    private  readonly _dashboardUrl: string,
    private readonly _notificationService: NotificationService)
    { }


  private loadBookmarks(): Observable<Bookmark[]> {
    return this._bookmarkClient
      .get<IDataResponseOf<Bookmark[]>>(`${this._dashboardUrl}/api/bookmarks`)
      .pipe(
        map(mapResponseOf),
        tap(result => this.bookmarkUpdates$.next(result))
      );
  }

  public getBookmarks(): Observable<Bookmark[]> {
    return this.loadBookmarks()
      .pipe(switchMap(r => {
        return this.bookmarkUpdates$.asObservable();
      }));

    // return this.loadBookmarks()
    //   .subscribe(r => {
    //     return this.bookmarkUpdates$.asObservable();
    //   });
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
        switchMap(r => this.loadBookmarks())
      );
  }

  public updateBookmark(todo: Bookmark) {
    return this._bookmarkClient
      .put<IDataResponseOf<Bookmark>>(`${this._dashboardUrl}/api/bookmarks`, todo)
      .pipe(
        map(mapResponseOf),
        tap(() => this._notificationService.show('Bookmark updated')),
        switchMap(r => this.loadBookmarks())
      );
  }

  public deleteBookmark(id: number) {
    return this._bookmarkClient
      .delete<IResponse>(`${this._dashboardUrl}/api/bookmarks/${id}`)
      .pipe(
        map(mapResponse),
        tap(r => this._notificationService.show('Bookmark deleted')),
        switchMap(r => this.loadBookmarks())
      );
  }
}
