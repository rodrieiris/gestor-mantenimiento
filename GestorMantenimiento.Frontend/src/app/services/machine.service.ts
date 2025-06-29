import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Machine } from '../models/machine.model';

@Injectable({ providedIn: 'root' })

export class MachineService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5294/api/machine';

  getAll(): Observable<Machine[]> {
    return this.http.get<Machine[]>(this.baseUrl);
  }
  create(m: Omit<Machine, 'id'>): Observable<Machine> {
    return this.http.post<Machine>(this.baseUrl, m);
  }
  update(m: Machine): Observable<Machine> {
    return this.http.put<Machine>(`${this.baseUrl}/${m.id}`, m);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}