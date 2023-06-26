import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Log } from './models/Log';
import { Tarefa } from './models/Tarefa';
import { TipoTarefa } from './models/TipoTarefa';
import { Usuario } from './models/Usuario';
import { SignalrService } from './services/signalr.service';
import { TarefaService } from './services/tarefa.service';
import { TipoTarefaService } from './services/tipo-tarefa.service';
import { UsuarioService } from './services/usuario.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  public logs = signal<Log[]>([]);
  public tarefas = signal<Tarefa[]>([]);
  public usuarios = signal<Usuario[]>([]);
  public tipoTarefas = signal<TipoTarefa[]>([]);
  public signalrStatus = signal<boolean>(false);

  public selectUsuario = signal<Usuario[]>([]);
  public selectTipoTarefa = signal<TipoTarefa[]>([]);

  public tarefaForm = this.formBuilder.group({
    id: [''],
    descricao: ['', [Validators.required]],
    idtipotarefa: ['', [Validators.required]],
    idusuario: ['', [Validators.required]],
    datatarefa: ['', [Validators.required]],
    feito: false
  });

  public tipoTarefaForm = this.formBuilder.group({
    descricao: ['', [Validators.required]]
  });

  public usuarioForm = this.formBuilder.group({
    nome: ['', [Validators.required]],
    nomeUsuario: ['', [Validators.required]],
    senha: ['', [Validators.required]],
    email: ['', [Validators.required]]
  });

  constructor(
    private formBuilder: FormBuilder,
    private tarefaService: TarefaService,
    private usuarioService: UsuarioService,
    private tipoTarefaService: TipoTarefaService,
    private signalrService: SignalrService) {
      this.signalrService.eventEmitter.subscribe((log: Log) => {
        this.logs.update((value: Log[]) => [...value, log]);
      });
    }

  ngOnInit(): void {
    this.getTarefas();
    this.getUsuarios();
    this.getTipoTarefas();

    this.signalrService.startConnection().then((conectado: boolean) => {
      if (conectado) {
        this.signalrStatus.set(conectado);
        this.signalrService.addTransferDataListener();
        this.signalrService.startHttpRequest();
      }
    });
  }

  private async getTarefas(): Promise<void> {
    this.tarefas.set(await this.tarefaService.get() as Tarefa[]);
  }

  private async getTipoTarefas(): Promise<void> {
    this.tipoTarefas.set(await this.tipoTarefaService.get() as TipoTarefa[]);
    this.selectTipoTarefa.set(this.tipoTarefas());
  }

  private async getUsuarios(): Promise<void> {
    this.usuarios.set(await this.usuarioService.get() as Usuario[]);
    this.selectUsuario.set(this.usuarios());
  }

  public async cadastrarTarefa(): Promise<void> {
    if (this.tarefaForm.valid) {
      const item = this.tarefaForm.getRawValue();
      const res = await this.tarefaService.post(item as Tarefa);
      if (res)
        this.getTarefas();
    }
  }

  public async excluirTarefa(id: string): Promise<void> {
    const res = await this.tarefaService.delete(id);
    if (res)
      this.getTarefas();
  }

  public async mudarStatusTarefa(item: Tarefa): Promise<void> {
    item.feito = !item.feito;

    const res = await this.tarefaService.put(item);
    if (res)
      this.getTarefas();
  }

  public async cadastrarTipoTarefa(): Promise<void> {
    if (this.tipoTarefaForm.valid) {
      const item = this.tipoTarefaForm.getRawValue();
      const res = await this.tipoTarefaService.post(item);
      if (res)
        this.getTipoTarefas();
    }
  }

  public async excluirTipoTarefa(id: string): Promise<void> {
    const res = await this.tipoTarefaService.delete(id);
    if (res)
      this.getTipoTarefas();
  }

  public async cadastrarUsuario(): Promise<void> {
    if (this.usuarioForm.valid) {
      const item = this.usuarioForm.getRawValue();
      const res = await this.usuarioService.post(item);
      if (res)
        this.getUsuarios();
    }
  }

  public async excluirUsuario(id: string): Promise<void> {
    const res = await this.usuarioService.delete(id);
    if (res)
      this.getUsuarios();
  }
}
