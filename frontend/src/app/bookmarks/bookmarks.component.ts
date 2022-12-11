import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Bookmark } from '../models/bookmark.model';
import { ButtonType, ControlType, FieldType } from '../models/dynamic-form.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrls: ['./bookmarks.component.scss']
})
export class BookmarksComponent implements OnInit {

  public bookmarks: Bookmark[] = [];

  constructor(
    private readonly _bookmarkService: BookmarkService,
    private readonly _router: Router,
    private readonly _stateService: DynamicFormStateService) { }

  ngOnInit(): void {
    this.bookmarks = this._bookmarkService.getBookmarks();
  }

  public onAdd(): void {
    this._router.navigate(['/form']);
    this._stateService.push({
      title: 'New Bookmark',
      controls: [
        {
          label: 'Name',
          key: 'name',
          fieldType: FieldType.text,
          controlType: ControlType.input,
          required: true,
          visible: true,
        },
        {
          label: 'URL',
          key: 'url',
          fieldType: FieldType.url,
          controlType: ControlType.input,
          required: true,
          visible: true,
        }
      ],
      buttons: [
        {
          label: 'Cancel',
          type: ButtonType.link,
          linkUrl: '/bookmarks'
        },
        {
          label: 'Save',
          type: ButtonType.submit,
          alignEnd: true,
          onClick: this.onBookmarkAdded
        }
      ]
    });
  }

  private onBookmarkAdded = (value: object): void => {
    const { name, url } = value as any;
    const bookmark = new Bookmark(name, url);
    this._bookmarkService.addBookmark(bookmark);
    this._router.navigate(['/bookmarks']);
  }
}
