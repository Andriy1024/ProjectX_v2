import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Bookmark } from '../models/bookmark.model';
import { ButtonType, ControlType, FieldType } from '../models/dynamic-form.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';

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
    private router: Router,
    private readonly _stateService: DynamicFormStateService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      const bookmarkId = paramMap.get('id');
      if (!bookmarkId) {
        return;
      }

      this.bookmark = this.bookmarkService.findBookmark(bookmarkId);
      if (!this.bookmark) {
        return;
      }

      this._stateService.push({
        title: 'Edit Bookmark',
        controls: [
          {
            label: 'Id',
            key: 'id',
            fieldType: FieldType.number,
            controlType: ControlType.input,
            required: true,
            visible: false,
          },
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
            linkUrl: '/bookmarks/manage'
          },
          {
            label: 'Delete',
            type: ButtonType.button,
            onClick: this.onDeleted
          },
          {
            label: 'Save',
            type: ButtonType.submit,
            alignEnd: true,
            onClick: this.onUpdated
          }
        ],
        data: this.bookmark
      });

    });
  }

  public onUpdated = (value: object): void => {
    const { name, url } = value as Bookmark;

    this.bookmarkService.updateBookmark(this.bookmark!.id, {
      name,
      url: new URL(url)
    });
  }

  public onDeleted = (value: object): void => {
    this.router.navigate(['../'], { relativeTo: this.route });
    this.bookmarkService.deleteBookmark(this.bookmark!.id);
  }
}
