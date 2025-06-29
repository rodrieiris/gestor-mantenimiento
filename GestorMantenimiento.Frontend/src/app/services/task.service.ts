import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { MaintenanceTask } from '../models/task.model';
import { DashboardStats } from '../models/dashboard-stats.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5294/api/maintenancetask';

  getAll(): Observable<MaintenanceTask[]> {
    return this.http.get<MaintenanceTask[]>(this.baseUrl);
  }

  create(t: Omit<MaintenanceTask, 'id'>): Observable<MaintenanceTask> {
    return this.http.post<MaintenanceTask>(this.baseUrl, t);
  }

  update(t: MaintenanceTask): Observable<MaintenanceTask> {
    return this.http.put<MaintenanceTask>(`${this.baseUrl}/${t.id}`, t);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  markCompleted(id: number): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/complete`, {});
  }

  getStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.baseUrl}/stats`);
  }
}