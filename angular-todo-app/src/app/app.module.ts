import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { SignalrService } from './services/signalr.service';
import { TarefaService } from './services/tarefa.service';
import { TipoTarefaService } from './services/tipo-tarefa.service';
import { UsuarioService } from './services/usuario.service';
import { UsuarioListComponent } from "./components/usuario/list/usuario-list.component";
import { UsuarioFormComponent } from "./components/usuario/form/usuario-form.component";
import { TipotarefaListComponent } from './components/tipotarefa/list/tipotarefa-list.component';
import { TipotarefaFormComponent } from './components/tipotarefa/form/tipotarefa-form.component';
import { LogListComponent } from './components/log/list/log-list.component';
import { LogStatusComponent } from './components/log/status/log-status.component';
import { TarefaFormComponent } from './components/tarefa/form/tarefa-form.component';
import { TarefaListComponent } from './components/tarefa/list/tarefa-list.component';
import { NavComponent } from './components/layout/nav/nav.component';

@NgModule({
    declarations: [
      AppComponent
    ],
    providers: [
      TarefaService,
      UsuarioService,
      TipoTarefaService,
      SignalrService
    ],
    bootstrap: [AppComponent],
    imports: [
      BrowserModule,
      FlexLayoutModule,
      HttpClientModule,
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
    ]
})
export class AppModule { }
