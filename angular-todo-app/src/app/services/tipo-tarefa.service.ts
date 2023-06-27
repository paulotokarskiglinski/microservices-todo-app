import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { TipoTarefa } from '../models/TipoTarefa';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TipoTarefaService {

  private environment = environment;
  private readonly http = inject(HttpClient);

  public async get(): Promise<TipoTarefa[] | null> {
    try {
      return await firstValueFrom(
        this.http.get<TipoTarefa[]>(this.environment.urlApi + 'tipotarefa')
      );
    } catch (error: unknown) {
      throw error;
    }
  }

  public async post(item: any): Promise<TipoTarefa | null> {
    try {
      return await firstValueFrom(
        this.http.post<TipoTarefa>(this.environment.urlApi + 'tipotarefa', item)
      ); 
    } catch (error: unknown) {
      throw error;
    }
  }

  public async delete(id: string): Promise<TipoTarefa | null> {
    try {
      return await firstValueFrom(
        this.http.delete<TipoTarefa>(this.environment.urlApi + 'tipotarefa/' + id)
      );
    } catch (error: unknown) {
      throw error;
    }
  }
}
