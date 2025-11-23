import { Component, Input, OnInit } from '@angular/core';
import { Bookmark } from '../models/bookmark.model';

@Component({
    selector: 'app-bookmark-tile',
    templateUrl: './bookmark-tile.component.html',
    styleUrls: ['./bookmark-tile.component.scss'],
    standalone: false
})
export class BookmarkTileComponent implements OnInit {

  @Input() public bookmark: Bookmark | undefined;

  public tileIconSrc: string | undefined;

  public iconError: boolean = false;

  constructor() { }

  ngOnInit(): void {
    this.tileIconSrc = this.bookmark!.url + '/favicon.ico';
  }

}
