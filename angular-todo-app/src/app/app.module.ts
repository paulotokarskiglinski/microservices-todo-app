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

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FlexLayoutModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    TarefaService,
    UsuarioService,
    TipoTarefaService,
    SignalrService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
