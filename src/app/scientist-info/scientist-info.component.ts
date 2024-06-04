import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


import { ScientistScraperService } from '../_services/scientist-scraper.service';
import { ScientistInfo } from '../_models/scientist-info';

@Component({
  selector: 'app-scientist-info',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './scientist-info.component.html',
  styleUrls: ['./scientist-info.component.css']
})
export class ScientistInfoComponent {
  scientistName: string = 'Albert Einstein'; // Example
  scientistInfos: ScientistInfo[] = [];
  loading: boolean = false;
  error: string | null = null;
  constructor(private scientistScraperService: ScientistScraperService) { }

  getScientistInfo() {
    this.loading = true;
    this.error = null;
    this.scientistInfos = [];

    this.scientistScraperService.getScientistInfo(this.scientistName)
      .subscribe(
        infos => {
          this.scientistInfos = infos;
          this.loading = false;
        },
        error => {
          this.error = 'Failed to fetch scientist information';
          this.loading = false;
        }
      );
  }
}
