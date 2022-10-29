import { Component, OnInit } from '@angular/core';
import { Bookmark } from '../models/bookmark.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';

@Component({
  selector: 'app-bookmarks-manage',
  templateUrl: './bookmarks-manage.component.html',
  styleUrls: ['./bookmarks-manage.component.scss']
})
export class BookmarksManageComponent implements OnInit {

  bookmarks: Bookmark[] = [];

  constructor(private bookmarkService: BookmarkService) { }

  ngOnInit(): void {
    this.bookmarks = this.bookmarkService.getBookmarks();
  }

  public onBookmarkDelete(data: any) {
    console.log(data)
  }
}
