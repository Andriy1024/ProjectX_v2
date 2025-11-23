import { ChangeDetectionStrategy, Component, computed, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Bookmark } from '../models/bookmark.model';

@Component({
    selector: 'app-bookmark-tile',
    templateUrl: './bookmark-tile.component.html',
    styleUrls: ['./bookmark-tile.component.scss'],
    standalone: true,
    imports: [CommonModule],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookmarkTileComponent {

  public bookmark = input.required<Bookmark>();

  public tileIconSrc = computed(() => this.bookmark().url + '/favicon.ico');

  public iconError = signal(false);
}
