import { Component, OnInit } from '@angular/core';
import { Note } from '../models/note.model';
import { NoteService } from '../services/notes/note.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent implements OnInit {

  public notes: Note[] = [];

  constructor(private _noteService: NoteService) { }

  ngOnInit(): void {
    this.notes = this._noteService.getNotes();
  }

}
