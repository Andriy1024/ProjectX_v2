import { Injectable } from '@angular/core';
import { Bookmark } from 'src/app/models/bookmark.model';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService {

  private todos: Bookmark[] = [];

  constructor() { }

  public getBookmarks() {
    return this.todos;
  }

  public findBookmark(id: string) {
    return this.todos.find(t => t.id === id);
  }

  public addBookmark(todo: Bookmark) {
    this.todos.push(todo);
  }

  public updateBookmark(id: string, updatedFields: Partial<Bookmark>){
    const todo = this.findBookmark(id);
    if (todo) {
      Object.assign(todo, updatedFields);
    }
  }

  public deleteBookmark(id: string) {
    this.todos = this.todos.filter(t => t.id !== id);
  }
}
