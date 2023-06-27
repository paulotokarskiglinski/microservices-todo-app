import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventsEnum } from 'src/app/enums/Events';
import { Tarefa } from 'src/app/models/Tarefa';
import { TipoTarefa } from 'src/app/models/TipoTarefa';
import { Usuario } from 'src/app/models/Usuario';
import { EventEmitterService } from 'src/app/services/event-emitter.service';
import { TarefaService } from 'src/app/services/tarefa.service';
import { TipoTarefaService } from 'src/app/services/tipo-tarefa.service';
import { UsuarioService } from 'src/app/services/usuario.service';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, ReactiveFormsModule],
  selector: 'app-tarefa-form',
  templateUrl: './tarefa-form.component.html'
})
export class TarefaFormComponent implements OnInit {

  private readonly eventsEnum = EventsEnum;
  private readonly formBuilder = inject(FormBuilder);
  private readonly tarefaService = inject(TarefaService);
  private readonly usuarioService = inject(UsuarioService);
  private readonly tipoTarefaService = inject(TipoTarefaService);
  private readonly eventEmitterService = inject(EventEmitterService);
  
  public selectUsuarios = signal<Usuario[]>([]);
  public selectTipoTarefas = signal<TipoTarefa[]>([]);

  public tarefaForm = this.formBuilder.group({
    id: [''],
    descricao: ['', [Validators.required]],
    idtipotarefa: ['', [Validators.required]],
    idusuario: ['', [Validators.required]],
    datatarefa: ['', [Validators.required]],
    feito: false
  });

  constructor() {
    this.eventEmitterService.eventEmitter.subscribe((event: any) => {
      if (event === this.eventsEnum.TIPOTAREFA || event === this.eventsEnum.USUARIO)
        this.get();
    });
  }

  ngOnInit(): void {
    this.get();
  }

  public async cadastrar(): Promise<void> {
    if (this.tarefaForm.valid) {
      const item = this.tarefaForm.getRawValue();
      const res = await this.tarefaService.post(item as Tarefa);
      if (res)
        this.eventEmitterService.eventEmitter.emit(this.eventsEnum.TAREFA);
    }
  }

  private async get(): Promise<void> {
    this.selectUsuarios.set(await this.usuarioService.get() as Usuario[]);
    this.selectTipoTarefas.set(await this.tipoTarefaService.get() as TipoTarefa[]);
  }
}
