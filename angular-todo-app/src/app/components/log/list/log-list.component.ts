import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { Log } from 'src/app/models/Log';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-log-list',
  templateUrl: './log-list.component.html',
  imports: [DatePipe]
})
export class LogListComponent {
  private readonly signalrService = inject(SignalrService);

  public logs = signal<Log[]>([]);

  constructor() {
    this.signalrService.eventEmitter.subscribe((log: Log) => {
      this.logs.update((value: Log[]) => [log, ...value]);
    });
  }
}
