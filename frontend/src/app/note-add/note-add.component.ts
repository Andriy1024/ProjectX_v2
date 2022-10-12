import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Note } from '../models/note.model';
import { NoteService } from '../services/notes/note.service';

@Component({
  selector: 'app-note-add',
  templateUrl: './note-add.component.html',
  styleUrls: ['./note-add.component.scss']
})
export class NoteAddComponent implements OnInit {

  constructor(
    private _noteService: NoteService,
    private _router: Router) { }

  ngOnInit(): void {
  }

  public onFormSubmit(form: NgForm) {
    if(form.invalid)
      return alert("Form is invalid");

    const note = new Note(form.value.title, form.value.content);
    
    this._noteService.addNote(note);

    this._router.navigate(['/notes']);
  }

}
