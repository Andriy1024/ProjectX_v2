import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { from, Observable, of, switchMap, tap, catchError } from 'rxjs';
import { Bookmark } from '../models/bookmark.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';
import { LoggerService } from '../services/logging/logger.service';

@Component({
    selector: 'app-bookmarks-manage',
    templateUrl: './bookmarks-manage.component.html',
    styleUrls: ['./bookmarks-manage.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterModule],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookmarksManageComponent implements OnInit {
  public bookmarks$: Observable<Bookmark[]> = from([]);
  public readonly error = signal<string | null>(null);

  private readonly bookmarkService = inject(BookmarkService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly logger = inject(LoggerService);

  ngOnInit(): void {
    this.bookmarks$ = this.route.paramMap.pipe(
      switchMap((paramMap) => {
        let bookmarkId = paramMap.get('id');
        return this.bookmarkService.getBookmarks().pipe(
          tap(bookmarks => {
            let bookmark = !bookmarkId
              ? bookmarks[0]
              : bookmarks.find(x => x.id == Number(bookmarkId)) ?? bookmarks[0];

            if (bookmark)
              this.router.navigate([bookmark.id], {relativeTo: this.route});
            else
              this.router.navigateByUrl('/bookmarks');
          }),
          catchError(err => {
            this.logger.error('Failed to load bookmarks', err);
            this.error.set('Failed to load bookmarks.');
            return of([]);
          })
        );
      })
    );
  }
}
