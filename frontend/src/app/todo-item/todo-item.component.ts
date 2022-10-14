import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Todo } from '../models/todo.model';

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss']
})
export class TodoItemComponent implements OnInit {

  @Input() public todo!: Todo;

  @Output() public editClick: EventEmitter<void> = new EventEmitter();

  @Output() public deleteClick: EventEmitter<void> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  public onEditClick() {
    this.editClick.emit();
  }

  public onDeleteClick() {
    this.deleteClick.emit();
  }
}
