import { Routes } from '@angular/router';
import { MachinesComponent } from './pages/machines/machines.component/machines.component';
import { TasksComponent } from './pages/tasks/tasks.component/tasks.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component/dashboard.component';

export const routes: Routes = [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'machines', component: MachinesComponent },
    { path: 'tasks', component: TasksComponent },
    { path: 'dashboard', component: DashboardComponent }
];