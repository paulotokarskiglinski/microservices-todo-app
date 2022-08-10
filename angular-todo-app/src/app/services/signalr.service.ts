import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as SignalR from '@microsoft/signalr';
import { map, shareReplay } from 'rxjs';
import { Log } from '../models/Log';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  public eventEmitter: EventEmitter<Log>;
  private hubConnection!: SignalR.HubConnection;
  private environment = environment;

  public addTransferDataListener = () => {
    this.hubConnection.on('LogMessage', (data: Log) => {
      this.eventEmitter.emit(data);
    });
  }

  constructor(private http: HttpClient) {
    this.eventEmitter = new EventEmitter<Log>();
  }

  public startConnection(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      if (this.hubConnection) {
        resolve(true);
      }

      this.hubConnection = new SignalR.HubConnectionBuilder()
        .withUrl(this.environment.urlSignalr + 'LogHub')
        .build();

      this.hubConnection.start()
        .then(() => {
          resolve(true);
        })
        .catch(err =>  {
          resolve(false);
          throw err;
        });
    });
  }

  public startHttpRequest(): any {
    this.http.get(this.environment.urlSignalr + 'LogHub').pipe(
      map((res: any) => {
        return res;
      }),
      shareReplay()
    );
  }
}
