import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Todo } from '../models/todo.model';
import { TodoService } from '../services/todo/todo.service';

@Component({
  selector: 'app-todo-edit',
  templateUrl: './todo-edit.component.html',
  styleUrls: ['./todo-edit.component.scss']
})
export class TodoEditComponent implements OnInit {

  public todo: Todo | undefined;

  constructor(
    private readonly todoService: TodoService,
    private readonly router: Router,
    private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
      const id = paramMap.get('id')!;
      this.todo = this.todoService.findTodo(id);
    });
  }

  public onFormSubmit(form: NgForm) {
    this.todoService.updateTodo(this.todo!.id, form.value);
    this.router.navigate(['/todos']);
  }

}
