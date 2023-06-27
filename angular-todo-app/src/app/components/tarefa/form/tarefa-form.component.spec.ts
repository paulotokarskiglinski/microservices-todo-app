import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TarefaFormComponent } from './tarefa-form.component';

describe('TarefaFormComponent', () => {
  let component: TarefaFormComponent;
  let fixture: ComponentFixture<TarefaFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TarefaFormComponent]
    });
    fixture = TestBed.createComponent(TarefaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
