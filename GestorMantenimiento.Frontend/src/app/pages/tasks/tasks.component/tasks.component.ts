import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { TaskService } from '../../../services/task.service';
import { MaintenanceTask } from '../../../models/task.model';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [DatePipe],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent implements OnInit {
  tasks: MaintenanceTask[] = [];
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private svc: TaskService,
    private datePipe: DatePipe
  ) {
    this.form = this.fb.group({
      id: [0],
      description: ['', Validators.required],
      scheduledDate: ['', Validators.required],
      machineId: [0, [Validators.required, Validators.min(1)]],
      isCompleted: [false]
    });
  }

  ngOnInit() {
    this.loadAll();
  }

  private loadAll() {
    this.svc.getAll().subscribe(
      data => (this.tasks = data),
      (err: any) => console.error('Error cargando tareas', err)
    );
  }

  submit() {
    const raw = this.form.getRawValue();
    const payload = {
      description: raw.description,
      scheduledDate: new Date(raw.scheduledDate).toISOString(),
      machineId: raw.machineId,
      isCompleted: raw.isCompleted
    };

    let call$;
    if (raw.id > 0) {
      call$ = this.svc.update({ id: raw.id, ...payload });
    } else {
      call$ = this.svc.create(payload);
    }

    call$.subscribe(
      () => {
        this.form.reset({
          id: 0,
          description: '',
          scheduledDate: '',
          machineId: 0,
          isCompleted: false
        });
        this.loadAll();
      },
      (err: any) => console.error('Error guardando tarea', err)
    );
  }

  edit(t: MaintenanceTask) {
    const iso = this.datePipe.transform(t.scheduledDate, 'yyyy-MM-dd')!;
    this.form.setValue({
      id: t.id,
      description: t.description,
      scheduledDate: iso,
      machineId: t.machineId,
      isCompleted: t.isCompleted
    });
  }

  remove(id: number) {
    this.svc.delete(id).subscribe(
      () => this.loadAll(),
      (err: any) => console.error('Error eliminando tarea', err)
    );
  }

  markCompleted(id: number) {
    this.svc.markCompleted(id).subscribe(
      () => this.loadAll(),
      (err: any) => console.error('Error completando tarea', err)
    );
  }
}