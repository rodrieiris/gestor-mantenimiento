import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component/dashboard.component';
import { MachinesComponent } from './pages/machines/machines.component/machines.component';
import { TasksComponent } from './pages/tasks/tasks.component/tasks.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'machines', component: MachinesComponent },
    { path: 'tasks', component: TasksComponent },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: '**', redirectTo: 'login' }
];