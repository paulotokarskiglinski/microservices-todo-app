import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Log } from 'src/app/models/Log';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule],
  selector: 'app-log-list',
  templateUrl: './log-list.component.html'
})
export class LogListComponent {
  
  private readonly signalrService = inject(SignalrService);

  public logs = signal<Log[]>([]);

  constructor() {
    this.signalrService.eventEmitter.subscribe((log: Log) => {
      this.logs.update((value: Log[]) => [...value, log]);
    });
  }
}
