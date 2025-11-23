import { animate, style, transition, trigger } from '@angular/animations';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { ButtonType, ControlType, FieldType } from '../../models/dynamic-form.model';
import { Todo } from '../../models/todo.model';
import { DynamicFormStateService } from '../../services/dynamic-form/DynamicFormStateService';
import { TodoService } from '../../services/todo/todo.service';
import { LoggerService } from '../../services/logging/logger.service';

@Component({
    selector: 'app-todos',
    templateUrl: './todos.component.html',
    styleUrls: ['./todos.component.scss'],
    standalone: true,
    imports: [CommonModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: [
        trigger('todoItemAnimation', [
            transition(':leave', [
                animate(200, style({
                    opacity: 0,
                    height: 0,
                    marginBottom: 0
                }))
            ])
        ])
    ]
})
export class TodosComponent {
  // Modern Angular: Using signals instead of Observables
  private readonly _todoService = inject(TodoService);
  private readonly _router = inject(Router);
  private readonly _stateService = inject(DynamicFormStateService);
  private readonly _logger = inject(LoggerService);

  // Convert Observable to Signal using toSignal()
  public readonly todos = toSignal(this._todoService.getTodos(), { initialValue: [] });
  
  // Computed signal - automatically updates when todos changes
  public readonly completedCount = computed(() => 
    this.todos().filter(todo => todo.completed).length
  );
  
  public readonly activeCount = computed(() => 
    this.todos().filter(todo => !todo.completed).length
  );

  // Loading and error state signals
  public readonly loading = signal(false);
  public readonly error = signal<string | null>(null);

  public toggleCompleted(todo: Todo): void {
    todo.completed = !todo.completed;
    this._todoService.updateTodo(todo).subscribe({
      error: (err) => {
        this._logger.error('Failed to update todo', err);
        this.error.set('Failed to update todo. Please try again.');
        // Revert the change
        todo.completed = !todo.completed;
      }
    });
  }

  public onDeleteClick(todo: Todo): void {
    this._todoService.deleteTodo(todo.id).subscribe({
      error: (err) => {
        this._logger.error('Failed to delete todo', err);
        this.error.set('Failed to delete todo. Please try again.');
      }
    });
  }

  public onEditClick(todo: Todo): void {
    this._router.navigate(['/form']);
    this._stateService.push({
      title: 'Edit Todo',
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
          label: 'Name',
          key: 'name',
          fieldType: FieldType.text,
          controlType: ControlType.input,
          required: true,
          visible: true,
        }
      ],
      buttons: [
        {
          label: 'Cancel',
          type: ButtonType.link,
          linkUrl: '/todos'
        },
        {
          label: 'Save',
          type: ButtonType.submit,
          alignEnd: true,
          onClick: this.onTodoUpdated
        }
      ],
      data: todo
    });
  }

  public onAdd(): void {
    this._router.navigate(['/form']);
    this._stateService.push({
      title: 'New Todo',
      controls: [
        {
          label: 'Name',
          key: 'name',
          fieldType: FieldType.text,
          controlType: ControlType.input,
          required: true,
          visible: true,
        }
      ],
      buttons: [
        {
          label: 'Cancel',
          type: ButtonType.link,
          linkUrl: '/todos'
        },
        {
          label: 'Save',
          type: ButtonType.submit,
          alignEnd: true,
          onClick: this.onTodoAdded
        }
      ]
    });
  }

  private onTodoAdded = (value: object): void => {
    this.loading.set(true);
    this.error.set(null);
    
    this._todoService.addTodo(value as Todo).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/todos']);
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to add todo', err);
        this.error.set('Failed to add todo. Please try again.');
      }
    });
  }

  private onTodoUpdated = (value: object): void => {
    this.loading.set(true);
    this.error.set(null);
    
    this._todoService.updateTodo(value as Todo).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/todos']);
      },
      error: (err) => {
        this.loading.set(false);
        this._logger.error('Failed to update todo', err);
        this.error.set('Failed to update todo. Please try again.');
      }
    });
  }
}
