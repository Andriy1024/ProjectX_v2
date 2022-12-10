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

  public onAdd():void {
    console.log('on add note');
    this._router.navigate(['/form']);

    const onNoteCreated = new EventEmitter<object>();
    onNoteCreated.subscribe((value) => {
      console.log('on note save:' + value);

      const { title, content } = value as any;

      this._noteService.addNote(new Note(title, content));
      this._router.navigate(['/notes']);
    });

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
          required: false,
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
          onClick: onNoteCreated
        }
      ]
    });
  }

}
