import { TipoTarefa } from "./TipoTarefa";
import { Usuario } from "./Usuario";

export class Tarefa {
  public id!: string;
  public descricao!: string;
  public feito!: boolean;
  public dataTarefa!: string;
  public tipoTarefa?: TipoTarefa;
  public usuario?: Usuario;
}
