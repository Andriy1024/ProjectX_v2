import { Injectable } from '@angular/core';
import { Todo } from 'src/app/models/todo.model';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  private todos: Todo[] = [
    new Todo('first to do', true),
    new Todo('2aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxx  xxxxxxxxxxxxxxxxxxxxxxsssssssssssssssssssssssssssssssss', false)
  ];

  constructor() { }

  public getTodos() {
    return this.todos;
  }

  public findTodo(id: string) {
    return this.todos.find(t => t.id === id);
  }

  public addTodo(todo: Todo) {
    this.todos.push(todo);
  }

  public updateTodo(id: string, updatedFields: Partial<Todo>){
    const todo = this.findTodo(id);
    if (todo) {
      Object.assign(todo, updatedFields);
    }
  }

  public deleteTodo(id: string) {
    this.todos = this.todos.filter(t => t.id !== id);
  }
}
