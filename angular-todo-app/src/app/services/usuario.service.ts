import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Usuario } from '../models/Usuario';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private enviroiment = environment;

  constructor(private http: HttpClient) { }

  public async get(): Promise<Usuario[] | null> {
    try {
      return await firstValueFrom(
        this.http.get<Usuario[]>(this.enviroiment.urlApi + 'usuario')
      );
    } catch (error: unknown) {
      throw error;
    }
  }

  public async post(item: any): Promise<Usuario | null> {
    try {
      return await firstValueFrom(
        this.http.post<Usuario>(this.enviroiment.urlApi + 'usuario', item)
      );
    } catch (error: unknown) {
      throw error;
    }
  }

  public async delete(id: string): Promise<Usuario | null> {
    try {
      return await firstValueFrom(
        this.http.delete<Usuario>(this.enviroiment.urlApi + 'usuario/' + id)
      );
    } catch (error: unknown) {
      throw error;
    }
  }
}
