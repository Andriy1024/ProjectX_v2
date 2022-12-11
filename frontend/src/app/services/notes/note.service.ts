import { Injectable } from '@angular/core';
import { Note } from 'src/app/models/note.model';
import { NotificationData } from 'src/app/models/notification-data.model';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  public notes: Note[] = [
    new Note("note 1", "content 1"),
    new Note("note 2", "content 2aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxsssssssssssssssssssssssssssssssss")
  ];

  constructor(private readonly _notificationService: NotificationService) { }

  public getNotes(): Note[] {
    return this.notes;
  }

  public findNote(id: string): Note | undefined {
    return this.notes.find(n => n.id === id);
  }

  public addNote(note: Note) {
    this.notes.push(note);
    this._notificationService.show('Note created');
  }

  public updateNote(id: string, updateFields: Partial<Note>) {
    const note = this.findNote(id);

    if (note) {
      Object.assign(note, updateFields);
    }

    this._notificationService.show('Note updated');
  }

  public deleteNote(id: string) {
    this.notes = this.notes.filter(n => n.id !== id);
    this._notificationService.show('Note deleted');
  }
}
