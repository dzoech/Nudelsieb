import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

interface Todo {
  title: string;
}

@Component({
  selector: 'nudelsieb-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  todos: Todo[] = [{ title: 'Todo 1' }, { title: 'Todo 2' }];

  constructor(private http: HttpClient) {
    this.fetch();
  }
  fetch() {
    this.http.get<Todo[]>('https://localhost:5001/braindump/Neuron').subscribe((t) => (this.todos = t));
  }

  addTodo() {
    this.todos.push({
      title: `New todo ${Math.floor(Math.random() * 1000)}`,
    });
  }
}
