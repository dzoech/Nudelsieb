import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { apiConfig } from '../aadb2c-config';

@Component({
  selector: 'nudelsieb-neurons',
  templateUrl: './neurons.component.html',
  styleUrls: ['./neurons.component.scss'],
})
export class NeuronsComponent implements OnInit {
  neurons = [{ name: 'neuron 1' }, { name: 'neuron two' }];

  constructor(private http: HttpClient) {
    this.fetch();
  }
  fetch() {
    this.http.get(apiConfig.uri).subscribe((t) => console.log(t));
  }

  addTodo() {
    this.neurons.push({
      name: `New todo ${Math.floor(Math.random() * 1000)}`,
    });
  }

  ngOnInit(): void {}
}
