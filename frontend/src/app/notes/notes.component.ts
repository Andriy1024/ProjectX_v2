import { Component, EventEmitter, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonType, ControlType, FieldType, IButton } from '../models/dynamic-form.model';
import { Note } from '../models/note.model';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';
import { NoteService } from '../services/notes/note.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent implements OnInit {

  public notes: Note[] = [];

  constructor(
    private readonly _noteService: NoteService,
    private readonly _router: Router,
    private readonly _stateService: DynamicFormStateService) { }

  ngOnInit(): void {
    this.notes = this._noteService.getNotes();
  }

  public onNoteUpdated = (note: Object) => {
    const { id, title, content } = note as Note;
    this._noteService.updateNote(id, {title, content});
    this._router.navigate(['/notes']);
  }

  public onNoteDeleted = (note: Object): void => {
    console.log('onNoteDeleted');
    const { id } = note as Note;
    this._noteService.deleteNote(id);
    this._router.navigate(['/notes']);
  }

  public onNoteAdded = (note: Object): void => {
    const { title, content } = note as Note;
    this._noteService.addNote(new Note(title, content));
    this._router.navigate(['/notes']);
  }

  public onEdit(note: Note) {
    this._router.navigate(['/form']);

    this._stateService.push({
      title: 'Update Note',
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
          label: 'Title',
          key: 'title',
          fieldType: FieldType.text,
          controlType: ControlType.input,
          required: true,
          visible: true,
        },
        {
          label: 'Content',
          key: 'content',
          fieldType: FieldType.text,
          controlType: ControlType.textarea,
          required: true,
          visible: true,
        }
      ],
      buttons: [
        {
          label: 'Cancel',
          type: ButtonType.link,
          linkUrl: '/notes'
        },
        {
          label: 'Delete',
          type: ButtonType.button,
          onClick: this.onNoteDeleted
        },
        {
          label: 'Save',
          type: ButtonType.submit,
          alignEnd: true,
          onClick: this.onNoteUpdated
        }
      ],
      data: note
    });
  }

  public onAdd():void {
    this._router.navigate(['/form']);

    this._stateService.push({
      title: 'New Note',
      controls: [
        {
          label: 'Title',
          key: 'title',
          fieldType: FieldType.text,
          controlType: ControlType.input,
          required: true,
          visible: true,
        },
        {
          label: 'Content',
          key: 'content',
          fieldType: FieldType.text,
          controlType: ControlType.textarea,
          required: true,
          visible: true,
        }
      ],
      buttons: [
        {
          label: 'Cancel',
          type: ButtonType.link,
          linkUrl: '/notes'
        },
        {
          label: 'Save',
          type: ButtonType.submit,
          alignEnd: true,
          onClick: this.onNoteAdded
        }
      ]
    });
  }

}
