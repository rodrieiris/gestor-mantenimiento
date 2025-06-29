import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MachineService } from '../../../services/machine.service';
import { Machine } from '../../../models/machine.model';

@Component({
  selector: 'app-machines',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
  ],
  providers: [
    DatePipe
  ],
  templateUrl: './machines.component.html',
  styleUrls: ['./machines.component.scss']
})
export class MachinesComponent implements OnInit {
  machines: Machine[] = [];
  form: FormGroup;

  constructor(
    private svc: MachineService,
    private fb: FormBuilder,
    private dp: DatePipe
  ) {
    this.form = this.fb.group({
      id: [0],
      name: ['', { nonNullable: true }],
      serialNumber: ['', { nonNullable: true }],
      installationDate: ['', { nonNullable: true }],
      isActive: [true, { nonNullable: true }],
    });
  }

  ngOnInit() {
    this.loadAll();
  }

  private loadAll() {
    this.svc.getAll().subscribe({
      next: data => this.machines = data,
      error: err => console.error('Error cargando máquinas', err)
    });
  }

  submit() {
    console.log('submit() llamado, valor del form:', this.form.value);
    const m = this.form.getRawValue() as Machine;
    console.log('🖨️ Payload que voy a enviar:', m);
    const call$ = m.id > 0
      ? this.svc.update(m)
      : this.svc.create(m);

    call$.subscribe({
      next: res => {
        console.log('respuesta del servidor:', res);
        this.form.reset({ isActive: true, id: 0 });
        this.loadAll();
      },
      error: err => console.error('Error guardando máquina', err)
    });
  }

  edit(m: Machine) {
    const iso = this.dp.transform(m.installationDate, 'yyyy-MM-dd')!;
    this.form.setValue({
      id: m.id,
      name: m.name,
      serialNumber: m.serialNumber,
      installationDate: iso,
      isActive: m.isActive
    });
  }

  remove(id: number) {
    this.svc.delete(id).subscribe({
      next: () => this.loadAll(),
      error: err => console.error('Error eliminando máquina', err)
    });
  }
}