import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, switchMap, tap } from 'rxjs';
import { Bookmark } from '../models/bookmark.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';

@Component({
  selector: 'app-bookmarks-manage',
  templateUrl: './bookmarks-manage.component.html',
  styleUrls: ['./bookmarks-manage.component.scss']
})
export class BookmarksManageComponent implements OnInit {
  public bookmarks$: Observable<Bookmark[]> = of([]);

  constructor(
    private bookmarkService: BookmarkService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.bookmarks$ = this.route.paramMap.pipe(
      switchMap((paramMap) => {
        const bookmarkId = paramMap.get('id');
        return this.bookmarkService.getBookmarks().pipe(
          tap(bookmarks => {
            if (!bookmarkId) {
              const bookmark = bookmarks[0];
              if (bookmark) {
                this.router.navigate([bookmark.id], {relativeTo: this.route});
              }
            }
          })
        );
      })
    );
  }
}
