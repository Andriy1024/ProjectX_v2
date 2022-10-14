import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Todo } from '../models/todo.model';
import { TodoService } from '../services/todo/todo.service';

@Component({
  selector: 'app-todo-add',
  templateUrl: './todo-add.component.html',
  styleUrls: ['./todo-add.component.scss']
})
export class TodoAddComponent implements OnInit {

  constructor(
    private _todoService: TodoService,
    private _router: Router) { }

  ngOnInit(): void {
  }

  public onFormSubmit(form: NgForm) {
    if(form.invalid)
      return alert("Form is invalid");

    const todo = new Todo(form.value.text, false);
    
    this._todoService.addTodo(todo);

    this._router.navigate(['/todos']);
  }

}
