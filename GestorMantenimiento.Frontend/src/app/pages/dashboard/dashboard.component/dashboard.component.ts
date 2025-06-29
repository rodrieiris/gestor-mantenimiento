import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../../services/task.service';
import { DashboardStats } from '../../../models/dashboard-stats.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  stats?: DashboardStats;
  loading = false;

  constructor(private svc: TaskService) { }

  ngOnInit() {
    this.loadStats();
  }

  loadStats() {
    this.loading = true;
    this.svc.getStats().subscribe({
      next: s => {
        this.stats = s;
        this.loading = false;
      },
      error: err => {
        console.error('Error cargando estadísticas', err);
        this.loading = false;
      }
    });
  }
}