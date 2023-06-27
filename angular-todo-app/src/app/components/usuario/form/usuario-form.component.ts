import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventEmitterService } from 'src/app/services/event-emitter.service';
import { UsuarioService } from 'src/app/services/usuario.service';
import { EventsEnum } from 'src/app/enums/Events';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, ReactiveFormsModule],
  selector: 'app-usuario-form',
  templateUrl: './usuario-form.component.html'
})
export class UsuarioFormComponent {

  private readonly eventEnum = EventsEnum;
  private readonly formBuilder = inject(FormBuilder);
  private readonly usuarioService = inject(UsuarioService);
  private readonly eventEmitterService = inject(EventEmitterService);

  public usuarioForm = this.formBuilder.group({
    nome: ['', [Validators.required]],
    nomeUsuario: ['', [Validators.required]],
    senha: ['', [Validators.required]],
    email: ['', [Validators.required]]
  });

  public async cadastrarUsuario(): Promise<void> {
    if (this.usuarioForm.valid) {
      const item = this.usuarioForm.getRawValue();
      const res = await this.usuarioService.post(item);
      if (res)
        this.eventEmitterService.eventEmitter.emit(this.eventEnum.USUARIO);
    }
  }
}
