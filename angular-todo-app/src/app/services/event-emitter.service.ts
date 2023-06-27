import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {

  public eventEmitter: EventEmitter<any>;

  constructor() {
    this.eventEmitter = new EventEmitter<any>();
  }
}
