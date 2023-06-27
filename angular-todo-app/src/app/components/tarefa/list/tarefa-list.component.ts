import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { EventsEnum } from 'src/app/enums/Events';
import { Tarefa } from 'src/app/models/Tarefa';
import { EventEmitterService } from 'src/app/services/event-emitter.service';
import { TarefaService } from 'src/app/services/tarefa.service';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule],
  selector: 'app-tarefa-list',
  templateUrl: './tarefa-list.component.html'
})
export class TarefaListComponent implements OnInit {

  public tarefas = signal<Tarefa[]>([]);
  private readonly eventsEnum = EventsEnum;
  private readonly tarefaService = inject(TarefaService);
  private readonly eventEmitterService = inject(EventEmitterService);

  constructor() {
    this.eventEmitterService.eventEmitter.subscribe((event: any) => {
      if (event === this.eventsEnum.TAREFA)
        this.get();
    });
  }

  ngOnInit(): void {
    this.get();
  }

  private async get(): Promise<void> {
    this.tarefas.set(await this.tarefaService.get() as Tarefa[]);
  }

  public async excluir(id: string): Promise<void> {
    const res = await this.tarefaService.delete(id);
    if (res)
      this.get();
  }

  public async mudarStatusTarefa(item: Tarefa): Promise<void> {
    item.feito = !item.feito;

    const res = await this.tarefaService.put(item);
    if (res)
      this.get();
  }
}
