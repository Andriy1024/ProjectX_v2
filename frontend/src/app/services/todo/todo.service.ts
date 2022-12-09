import { Injectable } from '@angular/core';
import { Todo } from 'src/app/models/todo.model';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  private todos: Todo[] = [
    new Todo('first to do', true),
    new Todo('second to do', true),
    new Todo('third to do', true),
    new Todo('fourth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('fifth to do', true),
    new Todo('long text jdv;uevks jfsbvuiwewkev kjhjkWEBF  xxxxxxxxxxxxxxxxxxxxxxsssssssssssssssssssssssssssssssss', false)
  ];

  constructor(private notificationService: NotificationService) { }

  public getTodos() {
    return this.todos;
  }

  public findTodo(id: string) {
    return this.todos.find(t => t.id === id);
  }

  public addTodo(todo: Todo) {
    this.todos.push(todo);
    this.notificationService.show('Todo created');
  }

  public updateTodo(id: string, updatedFields: Partial<Todo>){
    const todo = this.findTodo(id);
    if (todo) {
      Object.assign(todo, updatedFields);
    }
    this.notificationService.show('Todo updated');
  }

  public deleteTodo(id: string) {
    this.todos = this.todos.filter(t => t.id !== id);
    this.notificationService.show('Todo deleted');
  }
}
