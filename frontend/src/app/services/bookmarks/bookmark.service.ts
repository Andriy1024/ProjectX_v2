import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Bookmark } from 'src/app/models/bookmark.model';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService {

  private bookmarks: Bookmark[] = [
    new Bookmark('Google', 'https://www.google.com/'),
    new Bookmark('YouTube', 'https://www.youtube.com/'),
    new Bookmark('Twitter', 'https://www.twitter.com/')
  ];

  private bookmarkUpdates$ = new BehaviorSubject<Bookmark[]>(this.bookmarks);

  constructor(private readonly _notificationService: NotificationService) { }

  public getBookmarks() {
    return this.bookmarkUpdates$.asObservable();
  }

  public findBookmark(id: string) {
    return this.bookmarks.find(t => t.id === id);
  }

  public addBookmark(todo: Bookmark) {
    this.bookmarks.push(todo);
    this.bookmarkUpdates$.next(this.bookmarks);
    this._notificationService.show('Bookmark created');
  }

  public updateBookmark(id: string, updatedFields: Partial<Bookmark>) {
    const todo = this.findBookmark(id);
    if (todo) {
      Object.assign(todo, updatedFields);
    }
    this._notificationService.show('Bookmark updated');
  }

  public deleteBookmark(id: string) {
    this.bookmarks = this.bookmarks.filter(t => t.id !== id);
    this.bookmarkUpdates$.next(this.bookmarks);
    this._notificationService.show('Bookmark deleted');
  }
}
