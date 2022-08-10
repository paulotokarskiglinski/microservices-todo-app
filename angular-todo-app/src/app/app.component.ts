import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
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
  
  public logs: Log[] = [];
  public tarefas!: Observable<Tarefa[]>;
  public usuarios!: Observable<Usuario[]>;
  public tipoTarefas!: Observable<TipoTarefa[]>;
  public signalrStatus: boolean = false;

  public selectUsuario!: Observable<Usuario[]>;
  public selectTipoTarefa!: Observable<TipoTarefa[]>;

  public tarefaForm = this.formBuilder.group({
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
        this.logs.push(log);
      });
    }

  ngOnInit(): void {
    this.getTarefas();
    this.getUsuarios();
    this.getTipoTarefas();

    this.signalrService.startConnection().then((conectado: boolean) => {
      if (conectado) {
        this.signalrStatus = conectado;
        this.signalrService.addTransferDataListener();
        this.signalrService.startHttpRequest();
      }
    });
  }

  private getTarefas(): void {
    this.tarefas = this.tarefaService.get();
  }

  private getTipoTarefas(): void {
    this.tipoTarefas = this.tipoTarefaService.get();
    this.selectTipoTarefa = this.tipoTarefas;
  }

  private getUsuarios(): void {
    this.usuarios = this.usuarioService.get();
    this.selectUsuario = this.usuarios;
  }

  public cadastrarTarefa(): void {
    if (this.tarefaForm.valid) {
      const item = this.tarefaForm.getRawValue();

      this.tarefaService.post(item).subscribe((res: Tarefa) => {
        if (res) {
          this.getTarefas();
        }
      });
    }
  }

  public excluirTarefa(id: string): void {
    this.tarefaService.delete(id).subscribe((res: any) => {
      if (res) {
        this.getTarefas();
      }
    });
  }

  public mudarStatusTarefa(item: Tarefa): void {
    item.feito = !item.feito;

    this.tarefaService.put(item).subscribe((res: Tarefa) => {
      if (res) {
        this.getTarefas();
      }
    });
  }

  public cadastrarTipoTarefa(): void {
    if (this.tipoTarefaForm.valid) {
      const item = this.tipoTarefaForm.getRawValue();

      this.tipoTarefaService.post(item).subscribe((res: TipoTarefa) => {
        if (res) {
          this.getTipoTarefas();
        }
      });
    }
  }

  public excluirTipoTarefa(id: string): void {
    this.tipoTarefaService.delete(id).subscribe((res: any) => {
      if (res) {
        this.getTipoTarefas();
      }
    });
  }

  public cadastrarUsuario(): void {
    if (this.usuarioForm.valid) {
      const item = this.usuarioForm.getRawValue();

      this.usuarioService.post(item).subscribe((res: Usuario) => {
        if (res) {
          this.getUsuarios();
        }
      });
    }
  }

  public excluirUsuario(id: string): void {
    this.usuarioService.delete(id).subscribe((res: any) => {
      if (res) {
        this.getUsuarios();
      }
    });
  }
}
