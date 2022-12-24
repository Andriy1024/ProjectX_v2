import { animate, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { ButtonType, ControlType, FieldType } from '../../models/dynamic-form.model';
import { Todo } from '../../models/todo.model';
import { DynamicFormStateService } from '../../services/dynamic-form/DynamicFormStateService';
import { TodoService } from '../../services/todo/todo.service';

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
  styleUrls: ['./todos.component.scss'],
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
export class TodosComponent implements OnInit {

  public todos: Observable<Todo[]> = from([]);

  constructor(
    private readonly _todoService: TodoService,
    private readonly _router: Router,
    private readonly _stateService: DynamicFormStateService) { }

  ngOnInit(): void {
    this.loadTodos();
  }

  public loadTodos(): void {
    this.todos = this._todoService.getTodos();
  }

  public toggleCompleted(todo: Todo): void {
    todo.completed = !todo.completed;
    this._todoService.updateTodo(todo)
        .subscribe(r => {
          this.loadTodos();
        });
  }

  public onDeleteClick(todo: Todo): void {
    this._todoService.deleteTodo(todo.id)
      .subscribe(r => {
        this.loadTodos();
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
          onClick: this.onNoteUpdated
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
          onClick: this.onNoteAdded
        }
      ]
    });
  }

  private onNoteAdded = (value: object): void => {
    const task = value as Todo;
    this._todoService.addTodo(task)
      .subscribe(r => {
        this._router.navigate(['/todos']);
      });
  }

  private onNoteUpdated = (value: object): void => {
    this._todoService.updateTodo(value as Todo)
      .subscribe(r => {
        this._router.navigate(['/todos']);
      });
  }
}
