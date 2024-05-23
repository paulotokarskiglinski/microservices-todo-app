import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NavComponent } from './components/layout/nav/nav.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LogListComponent } from './components/log/list/log-list.component';
import { LogStatusComponent } from './components/log/status/log-status.component';
import { TarefaFormComponent } from './components/tarefa/form/tarefa-form.component';
import { TarefaListComponent } from './components/tarefa/list/tarefa-list.component';
import { TipotarefaFormComponent } from './components/tipotarefa/form/tipotarefa-form.component';
import { TipotarefaListComponent } from './components/tipotarefa/list/tipotarefa-list.component';
import { UsuarioFormComponent } from './components/usuario/form/usuario-form.component';
import { UsuarioListComponent } from './components/usuario/list/usuario-list.component';
import { SignalrService } from './services/signalr.service';
import { TarefaService } from './services/tarefa.service';
import { TipoTarefaService } from './services/tipo-tarefa.service';
import { UsuarioService } from './services/usuario.service';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    ReactiveFormsModule,
    UsuarioListComponent,
    UsuarioFormComponent,
    TipotarefaListComponent,
    TipotarefaFormComponent,
    LogListComponent,
    LogStatusComponent,
    TarefaFormComponent,
    TarefaListComponent,
    NavComponent
  ],
  providers: [
    TarefaService,
    UsuarioService,
    TipoTarefaService,
    SignalrService
  ],
})
export class AppComponent { }
