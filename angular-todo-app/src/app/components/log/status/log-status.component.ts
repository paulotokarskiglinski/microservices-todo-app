import { Component, inject, signal } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-log-status',
  templateUrl: './log-status.component.html'
})
export class LogStatusComponent {
  private readonly signalrService = inject(SignalrService);

  public signalrStatus = signal<boolean>(false);

  constructor() {
    this.signalrService.startConnection().then((conectado: boolean) => {
      if (conectado) {
        this.signalrStatus.set(conectado);
        this.signalrService.addTransferDataListener();
        this.signalrService.startHttpRequest();
      }
    });
  }
}
