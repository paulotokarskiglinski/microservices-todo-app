import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule],
  selector: 'app-log-status',
  templateUrl: './log-status.component.html',
  styleUrls: ['./log-status.component.scss']
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
