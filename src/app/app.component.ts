import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScientistInfoComponent } from './scientist-info/scientist-info.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ScientistInfoComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent { }
