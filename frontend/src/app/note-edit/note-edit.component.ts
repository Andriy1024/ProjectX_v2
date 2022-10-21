import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Note } from '../models/note.model';
import { NoteService } from '../services/notes/note.service';

@Component({
  selector: 'app-note-edit',
  templateUrl: './note-edit.component.html',
  styleUrls: ['./note-edit.component.scss']
})
export class NoteEditComponent implements OnInit {

  public note: Note | undefined;

  constructor(
    private _route: ActivatedRoute, 
    private _router: Router, 
    private _notesService: NoteService) { }

  ngOnInit(): void {
    this._route.paramMap.subscribe((paramMap: ParamMap) => {
      const id = paramMap.get('id')!;
      this.note = this._notesService.findNote(id);
    });
  }

  public onFormSubmit(form: NgForm) {
    this._notesService.updateNote(this.note!.id, form.value);
    this._router.navigate(['/notes']);
  }

  public deleteNote() {
    this._notesService.deleteNote(this.note!.id);
    this._router.navigate(['/notes']);
  }
}
