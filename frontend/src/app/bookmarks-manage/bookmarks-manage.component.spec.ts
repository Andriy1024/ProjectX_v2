import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookmarksManageComponent } from './bookmarks-manage.component';

describe('BookmarksManageComponent', () => {
  let component: BookmarksManageComponent;
  let fixture: ComponentFixture<BookmarksManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookmarksManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookmarksManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
