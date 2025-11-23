import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Bookmark } from '../models/bookmark.model';
import { ButtonType, ControlType, FieldType } from '../models/dynamic-form.model';
import { BookmarkService } from '../services/bookmarks/bookmark.service';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';
import { DynamicFormModule } from '../components/dynamic-form/dynamic-form.module';
import { LoggerService } from '../services/logging/logger.service';

@Component({
    selector: 'app-bookmark-edit',
    templateUrl: './bookmark-edit.component.html',
    styleUrls: ['./bookmark-edit.component.scss'],
    standalone: true,
    imports: [CommonModule, DynamicFormModule],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookmarkEditComponent implements OnInit {

  bookmark: Bookmark | undefined;
  
  private readonly bookmarkService = inject(BookmarkService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly _stateService = inject(DynamicFormStateService);
  private readonly _logger = inject(LoggerService);

  public readonly loading = signal(false);
  public readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      const bookmarkId = paramMap.get('id');
      if (!bookmarkId) {
        return;
      }

      this.bookmarkService.findBookmark(Number(bookmarkId))
        .subscribe({
          next: (b) => {
            this.bookmark = b;
            if (this.bookmark) {
              this.pushFormConfig();
            }
          },
          error: (err) => {
            this._logger.error('Failed to load bookmark', err);
            this.error.set('Failed to load bookmark.');
          }
        });
    });
  }

  private pushFormConfig() {
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
  }

  public onUpdated = (value: object): void => {
    this.loading.set(true);
    this.error.set(null);
    
    this.bookmarkService.updateBookmark(value as Bookmark)
      .subscribe({
        next: () => {
          this.loading.set(false);
          // Optional: navigate or show success
        },
        error: (err) => {
          this.loading.set(false);
          this._logger.error('Failed to update bookmark', err);
          this.error.set('Failed to update bookmark.');
        }
      });
  }

  public onDeleted = (value: object): void => {
    this.loading.set(true);
    this.error.set(null);
    
    this.bookmarkService.deleteBookmark(this.bookmark!.id)
    .subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['../'], { relativeTo: this.route });
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to delete bookmark', err);
        this.error.set('Failed to delete bookmark.');
      }
    });
  }
}
