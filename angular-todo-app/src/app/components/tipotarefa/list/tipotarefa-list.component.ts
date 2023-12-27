import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { EventsEnum } from 'src/app/enums/Events';
import { TipoTarefa } from 'src/app/models/TipoTarefa';
import { EventEmitterService } from 'src/app/services/event-emitter.service';
import { TipoTarefaService } from 'src/app/services/tipo-tarefa.service';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule],
  selector: 'app-tipotarefa-list',
  templateUrl: './tipotarefa-list.component.html'
})
export class TipotarefaListComponent implements OnInit {

  public loading = signal<boolean>(true);
  public tipoTarefas = signal<TipoTarefa[]>([]);

  private readonly eventsEnum = EventsEnum;
  private readonly tipoTarefaService = inject(TipoTarefaService);
  private readonly eventEmitterService = inject(EventEmitterService);

  constructor() {
    this.eventEmitterService.eventEmitter.subscribe((event: any) => {
      if (event === this.eventsEnum.TIPOTAREFA)
        this.get();
    });
  }

  ngOnInit(): void {
    this.get();
  }

  private async get(): Promise<void> {
    this.loading.set(true);
    this.tipoTarefas.set(await this.tipoTarefaService.get() as TipoTarefa[]);
    this.loading.set(false);
  }
  
  public async excluir(id: string): Promise<void> {
    const res = await this.tipoTarefaService.delete(id);
    if (res) {
      this.get();
      this.eventEmitterService.eventEmitter.emit(this.eventsEnum.TIPOTAREFA);
    }
  }
}
