import { animate, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonType, ControlType, FieldType } from '../models/dynamic-form.model';
import { Todo } from '../models/todo.model';
import { DynamicFormStateService } from '../services/dynamic-form/DynamicFormStateService';
import { TodoService } from '../services/todo/todo.service';

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

  public todos: Todo[] = [];

  constructor(
    private readonly todoService: TodoService,
    private readonly router: Router,
    private readonly _stateService: DynamicFormStateService) { }

  ngOnInit(): void {
    this.todos = this.todoService.getTodos();
  }

  public toggleCompleted(todo: Todo) {
    this.todoService.updateTodo(todo.id, { completed: !todo.completed });
  }

  public onEditClick(todo: Todo) {
    this.router.navigate(['/todos', todo.id]);
  }

  public onDeleteClick(todo: Todo) {
    this.todoService.deleteTodo(todo.id);
    this.todos = this.todoService.getTodos();
  }

  public onAdd(): void {
    this.router.navigate(['/form']);

    this._stateService.push({
      title: 'New Todo',
      controls: [
        {
          label: 'Text',
          key: 'text',
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
    const { text } = value as Todo;
    this.todoService.addTodo(new Todo(text, false));
    this.router.navigate(['/todos']);
  }
}
