import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Bookmark } from '../models/bookmark.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';

@Component({
  selector: 'app-bookmark-add',
  templateUrl: './bookmark-add.component.html',
  styleUrls: ['./bookmark-add.component.scss']
})
export class BookmarkAddComponent implements OnInit {
  constructor(
    private readonly bookmarkService: BookmarkService,
    private readonly router: Router) { }

  ngOnInit(): void {
  }

  public onFormSubmit(form: NgForm) {
    const { name, url } = form.value; 
    const bookmark = new Bookmark(name, url);
    this.bookmarkService.addBookmark(bookmark);
    this.router.navigate(['/bookmarks']);
  }
}
