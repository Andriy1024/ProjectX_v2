import { Injectable } from '@angular/core';
import { Bookmark } from 'src/app/models/bookmark.model';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService {

  private bookmarks: Bookmark[] = [
    new Bookmark('Google', 'https://www.google.com/'),
    new Bookmark('YouTube', 'https://www.youtube.com/'),
    new Bookmark('Twitter', 'https://www.twitter.com/')
  ];

  constructor() { }

  public getBookmarks() {
    return this.bookmarks;
  }

  public findBookmark(id: string) {
    return this.bookmarks.find(t => t.id === id);
  }

  public addBookmark(todo: Bookmark) {
    this.bookmarks.push(todo);
  }

  public updateBookmark(id: string, updatedFields: Partial<Bookmark>){
    const todo = this.findBookmark(id);
    if (todo) {
      Object.assign(todo, updatedFields);
    }
  }

  public deleteBookmark(id: string) {
    this.bookmarks = this.bookmarks.filter(t => t.id !== id);
  }
}
