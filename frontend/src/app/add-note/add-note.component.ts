import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Note } from '../models/note.model';
import { NoteService } from '../services/notes/note.service';

@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.scss']
})
export class AddNoteComponent implements OnInit {

  constructor(
    private _noteService: NoteService,
    private _router: Router) { }

  ngOnInit(): void {
  }

  public onFormSubmit(form: NgForm) {
    const note = new Note(form.value.title, form.value.content);
    
    this._noteService.addNote(note);

    this._router.navigate(['/notes']);
  }

}
