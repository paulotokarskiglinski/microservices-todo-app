import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Tarefa } from '../models/Tarefa';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {

  private environment = environment;
  private readonly http = inject(HttpClient);

  public async get(): Promise<Tarefa[] | null> {
    try {
      return await firstValueFrom(
        this.http.get<Tarefa[]>(this.environment.urlApi + 'tarefa')
      );
    } catch (error: unknown) {
      throw error;
    }
  }

  public async post(item: Tarefa): Promise<Tarefa | unknown> {
    try {
      return await firstValueFrom(
        this.http.post<Tarefa>(this.environment.urlApi + 'tarefa', item)
      );
    } catch (error: unknown) {
      throw error;
    }
  }

  public async put(item: any): Promise<Tarefa | unknown> {
    try {
      return await firstValueFrom(
        this.http.put<Tarefa>(this.environment.urlApi + 'tarefa', item)
      );
    } catch (error: unknown) {
      throw error;
    }
  }

  public async delete(id: string): Promise<Tarefa | unknown> {
    try {
      return await firstValueFrom(
        this.http.delete<Tarefa>(this.environment.urlApi + 'tarefa/' + id)
      );
    } catch (error: unknown) {
      throw error;
    }
  }
}
