import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { ButtonType, ControlType, FieldType } from '../../models/dynamic-form.model';
import { Note } from '../../models/note.model';
import { DynamicFormStateService } from '../../services/dynamic-form/DynamicFormStateService';
import { NoteService } from '../../services/notes/note.service';
import { LoggerService } from '../../services/logging/logger.service';

@Component({
    selector: 'app-notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.scss'],
    standalone: true,
    imports: [CommonModule],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotesComponent implements OnInit {
  // Traditional Observable pattern (kept for comparison with Signals)
  private readonly _noteService = inject(NoteService);
  private readonly _router = inject(Router);
  private readonly _stateService = inject(DynamicFormStateService);
  private readonly _logger = inject(LoggerService);

  public notes$: Observable<Note[]> = from([]);
  
  // Loading and error state signals
  public readonly loading = signal(false);
  public readonly error = signal<string | null>(null);

  public ngOnInit(): void {
    this.notes$ = this._noteService.getNotes();
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

  public onAdd(): void {
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

  private onNoteUpdated = (note: object): void => {
    this.loading.set(true);
    this.error.set(null);
    
    this._noteService.updateNote(note as Note).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/notes']);
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to update note', err);
        this.error.set('Failed to update note. Please try again.');
      }
    });
  }

  private onNoteDeleted = (note: object): void => {
    this.loading.set(true);
    this.error.set(null);
    const typedNote = note as Note;
    this._noteService.deleteNote(typedNote.id).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/notes']);
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to delete note', err);
        this.error.set('Failed to delete note. Please try again.');
      }
    });
  }

  private onNoteAdded = (note: object): void => {
    this.loading.set(true);
    this.error.set(null);
    
    this._noteService.addNote(note as Note).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/notes']);
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to add note', err);
        this.error.set('Failed to add note. Please try again.');
      }
    });
  }
}
