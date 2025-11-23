import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { Bookmark } from '../models/bookmark.model';
import { ButtonType, ControlType, FieldType } from '../models/dynamic-form.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';
import { BookmarkTileComponent } from '../bookmark-tile/bookmark-tile.component';

@Component({
    selector: 'app-bookmarks',
    templateUrl: './bookmarks.component.html',
    styleUrls: ['./bookmarks.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterModule, BookmarkTileComponent]
})
export class BookmarksComponent {
  // Modern Angular: Using inject() and signals
  private readonly _bookmarkService = inject(BookmarkService);
  private readonly _router = inject(Router);
  private readonly _stateService = inject(DynamicFormStateService);

  // Convert Observable to Signal
  public readonly bookmarks = toSignal(this._bookmarkService.getBookmarks(), { initialValue: [] });
  
  // Computed signal for bookmark count
  public readonly bookmarkCount = computed(() => this.bookmarks().length);

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
    this._bookmarkService.addBookmark(value as Bookmark)
    .subscribe(r => {
      this._router.navigate(['/bookmarks']);
    });
  }
}
