import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventsEnum } from 'src/app/enums/Events';
import { EventEmitterService } from 'src/app/services/event-emitter.service';
import { TipoTarefaService } from 'src/app/services/tipo-tarefa.service';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule],
  selector: 'app-tipotarefa-form',
  templateUrl: './tipotarefa-form.component.html'
})
export class TipotarefaFormComponent {

  private readonly eventEnum = EventsEnum;
  private readonly formBuilder = inject(FormBuilder);
  private readonly tipoTarefaService = inject(TipoTarefaService);
  private readonly eventEmitterService = inject(EventEmitterService);

  public tipoTarefaForm = this.formBuilder.group({
    descricao: ['', [Validators.required]]
  });

  public async cadastrarTipoTarefa(): Promise<void> {
    if (this.tipoTarefaForm.valid) {
      const item = this.tipoTarefaForm.getRawValue();
      const res = await this.tipoTarefaService.post(item);
      if (res)
        this.eventEmitterService.eventEmitter.emit(this.eventEnum.TIPOTAREFA);
    }
  }
}
