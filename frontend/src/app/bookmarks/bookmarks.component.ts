import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { Bookmark } from '../models/bookmark.model';
import { ButtonType, ControlType, FieldType } from '../models/dynamic-form.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';
import { BookmarkTileComponent } from '../bookmark-tile/bookmark-tile.component';
import { LoggerService } from '../services/logging/logger.service';

@Component({
    selector: 'app-bookmarks',
    templateUrl: './bookmarks.component.html',
    styleUrls: ['./bookmarks.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterModule, BookmarkTileComponent],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookmarksComponent {
  // Modern Angular: Using inject() and signals
  private readonly _bookmarkService = inject(BookmarkService);
  private readonly _router = inject(Router);
  private readonly _stateService = inject(DynamicFormStateService);
  private readonly _logger = inject(LoggerService);

  // Convert Observable to Signal
  public readonly bookmarks = toSignal(this._bookmarkService.getBookmarks(), { initialValue: [] });
  
  // Computed signal for bookmark count
  public readonly bookmarkCount = computed(() => this.bookmarks().length);
  
  // Loading and error state signals
  public readonly loading = signal(false);
  public readonly error = signal<string | null>(null);

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
    this.loading.set(true);
    this.error.set(null);
    
    this._bookmarkService.addBookmark(value as Bookmark).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/bookmarks']);
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to add bookmark', err);
        this.error.set('Failed to add bookmark. Please try again.');
      }
    });
  }
}
