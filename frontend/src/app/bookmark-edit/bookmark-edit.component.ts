import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Bookmark } from '../models/bookmark.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';

@Component({
  selector: 'app-bookmark-edit',
  templateUrl: './bookmark-edit.component.html',
  styleUrls: ['./bookmark-edit.component.scss']
})
export class BookmarkEditComponent implements OnInit {

  bookmark: Bookmark | undefined;

  constructor(
    private bookmarkService: BookmarkService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      const bookmarkId = paramMap.get('id');
      if (bookmarkId) {
        this.bookmark = this.bookmarkService.findBookmark(bookmarkId);
      }
    });
  }

  public onFormSubmit(form: NgForm) {
    const { name, url } = form.value;

    this.bookmarkService.updateBookmark(this.bookmark!.id, {
      name,
      url: new URL(url)
    });
  }

  public delete() {
    this.bookmarkService.deleteBookmark(this.bookmark!.id);
    this.router.navigate(['../'], { relativeTo: this.route });
  }
}
