import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TipoTarefa } from '../models/TipoTarefa';
import { map, shareReplay } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TipoTarefaService {

  private environment = environment;

  constructor(private http: HttpClient) { }

  public get(): Observable<TipoTarefa[]> {
    return this.http.get(this.environment.urlApi + 'tipotarefa').pipe(
      map((res: any) => {
        return res;
      }),
      shareReplay()
    );
  }

  public post(item: any): Observable<TipoTarefa> {
    return this.http.post(this.environment.urlApi + 'tipotarefa', item).pipe(
      map((res: any) => {
        return res;
      })
    );
  }

  public delete(id: string): Observable<string> {
    return this.http.delete(this.environment.urlApi + 'tipotarefa/' + id).pipe(
      map((res: any) => {
        return res;
      })
    );
  }
}
