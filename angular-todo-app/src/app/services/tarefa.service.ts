import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tarefa } from '../models/Tarefa';
import { map, shareReplay } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private environment = environment;

  constructor(private http: HttpClient) { }

  public get(): Observable<Tarefa[]> {
    return this.http.get(this.environment.urlApi + 'tarefa').pipe(
      map((res: any) => {
        return res;
      }),
      shareReplay()
    );
  }

  public post(item: any): Observable<Tarefa> {
    return this.http.post(this.environment.urlApi + 'tarefa', item).pipe(
      map((res: any) => {
        return res;
      })
    );
  }

  public put(item: any): Observable<Tarefa> {
    return this.http.put(this.environment.urlApi + 'tarefa', item).pipe(
      map((res: any) => {
        return res;
      })
    );
  }

  public delete(id: string): Observable<Tarefa> {
    return this.http.delete(this.environment.urlApi + 'tarefa/' + id).pipe(
      map((res: any) => {
        return res;
      })
    );
  }
}
