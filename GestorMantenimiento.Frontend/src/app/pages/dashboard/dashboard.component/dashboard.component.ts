import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
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

  constructor(
    private svc: TaskService,
    private authService: AuthService,
    private router: Router
  ) { }

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
        this.loading = false;
        if (err.status === 401) {
          this.authService.logout();
          this.router.navigate(['/login']);
        } else {
          console.error('Error cargando estadísticas', err);
        }
      }
    });
  }
}