import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Usuario } from '../models/Usuario';
import { map, shareReplay } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private enviroiment = environment;

  constructor(private http: HttpClient) { }

  public get(): Observable<Usuario[]> {
    return this.http.get(this.enviroiment.urlApi + 'usuario').pipe(
      map((res: any) => {
        return res;
      }),
      shareReplay()
    );
  }

  public post(item: any): Observable<Usuario> {
    return this.http.post(this.enviroiment.urlApi + 'usuario', item).pipe(
      map((res: any) => {
        return res;
      })
    );
  }

  public delete(id: string): Observable<string> {
    return this.http.delete(this.enviroiment.urlApi + 'usuario/' + id).pipe(
      map((res: any) => {
        return res;
      })
    );
  }
}
