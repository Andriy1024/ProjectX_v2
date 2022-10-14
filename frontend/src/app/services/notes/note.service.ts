import { Injectable } from '@angular/core';
import { Note } from 'src/app/models/note.model';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  public notes: Note[] = [
    new Note("note 1", "content 1"),
    new Note("note 2", "content 2aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxsssssssssssssssssssssssssssssssss")
  ];

  constructor() { }

  public getNotes(): Note[] {
    return this.notes;
  }

  public findNote(id: string): Note | undefined {
    return this.notes.find(n => n.id === id);
  }

  public addNote(note: Note) {
    this.notes.push(note);
  }

  public updateNote(id: string, updateFields: Partial<Note>) {
    const note = this.findNote(id);

    if (note) {
      Object.assign(note, updateFields);
    }
  }

  public deleteNote(id: string) {
    this.notes = this.notes.filter(n => n.id !== id);
  }
}