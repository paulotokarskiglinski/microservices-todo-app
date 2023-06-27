import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TipotarefaFormComponent } from './tipotarefa-form.component';

describe('TipotarefaFormComponent', () => {
  let component: TipotarefaFormComponent;
  let fixture: ComponentFixture<TipotarefaFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TipotarefaFormComponent]
    });
    fixture = TestBed.createComponent(TipotarefaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
