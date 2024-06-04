import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ScientistInfo } from '../_models/scientist-info';

@Injectable({
  providedIn: 'root'
})
export class ScientistScraperService {
  private baseUrl = 'https://scholar.google.com/scholar?hl=en&q=';

  constructor(private http: HttpClient) { }

  getScientistInfo(scientistName: string): Observable<ScientistInfo[]> {
    const url = `${this.baseUrl}${encodeURIComponent(scientistName)}`;
    return this.http.get(url, { responseType: 'text' })
      .pipe(
        map(htmlContent => {
          const parser = new DOMParser();
          const doc = parser.parseFromString(htmlContent, 'text/html');
          const resultNodes = doc.querySelectorAll('.gs_r.gs_or.gs_scl');
          const results: ScientistInfo[] = [];

          resultNodes.forEach(node => {
            const titleNode = node.querySelector('.gs_rt a');
            const authorNode = node.querySelector('.gs_a');
            const snippetNode = node.querySelector('.gs_rs');

            if (titleNode && authorNode && snippetNode) {
              results.push({
                title: titleNode.textContent?.trim() || 'No title available',
                authorInfo: authorNode.textContent?.trim() || 'No author information available',
                snippet: snippetNode.textContent?.trim() || 'No snippet available',
                link: titleNode.getAttribute('href') || 'No link available'
              });
            }
          });

          return results;
        }),
        catchError(() => of([]))
      );
  }
}
