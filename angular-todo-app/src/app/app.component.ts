import { Component, OnInit, inject, signal } from '@angular/core';
import { Tarefa } from './models/Tarefa';
import { TipoTarefa } from './models/TipoTarefa';
import { UsuarioService } from './services/usuario.service';
import { Usuario } from './models/Usuario';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  private readonly usuarioService = inject(UsuarioService);
  
  public tarefas = signal<Tarefa[]>([]);
  public tipoTarefas = signal<TipoTarefa[]>([]);

  public selectUsuarios = signal<Usuario[]>([]);
  public selectTipoTarefa = signal<TipoTarefa[]>([]);

  async ngOnInit(): Promise<void> {
    // TODO: load selects
    this.selectUsuarios.set(await this.usuarioService.get() as Usuario[]);
  }

}
