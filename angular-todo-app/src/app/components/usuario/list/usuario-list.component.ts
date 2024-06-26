import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Usuario } from 'src/app/models/Usuario';
import { EventEmitterService } from 'src/app/services/event-emitter.service';
import { UsuarioService } from 'src/app/services/usuario.service';
import { EventsEnum } from 'src/app/enums/Events';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule],
  selector: 'app-usuario-list',
  templateUrl: './usuario-list.component.html'
})
export class UsuarioListComponent implements OnInit {

  public loading = signal<boolean>(true);
  public usuarios = signal<Usuario[]>([]);

  private readonly eventsEnum = EventsEnum;
  private readonly usuarioService = inject(UsuarioService);
  private readonly eventEmitterService = inject(EventEmitterService);

  constructor() {
    this.eventEmitterService.eventEmitter.subscribe((event: any) => {
      if (event === this.eventsEnum.USUARIO)
        this.get();
    });
  }

  ngOnInit(): void {
    this.get();
  }

  private async get(): Promise<void> {
    this.loading.set(true);
    this.usuarios.set(await this.usuarioService.get() as Usuario[]);
    this.loading.set(false);
  }

  public async excluir(id: string): Promise<void> {
    const res = await this.usuarioService.delete(id);
    if (res) {
      this.get();
      this.eventEmitterService.eventEmitter.emit(this.eventsEnum.USUARIO);
    }
  }
}
