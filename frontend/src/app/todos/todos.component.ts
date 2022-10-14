import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Todo } from '../models/todo.model';
import { TodoService } from '../services/todo/todo.service';

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
  styleUrls: ['./todos.component.scss']
})
export class TodosComponent implements OnInit {

  public todos: Todo[] = [];

  constructor(
    private readonly todoService: TodoService,
    private readonly router: Router) { }

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
  }

}
