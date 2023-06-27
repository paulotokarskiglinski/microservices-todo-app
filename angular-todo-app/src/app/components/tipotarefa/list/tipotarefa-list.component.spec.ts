import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TipotarefaListComponent } from './tipotarefa-list.component';

describe('TipotarefaListComponent', () => {
  let component: TipotarefaListComponent;
  let fixture: ComponentFixture<TipotarefaListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TipotarefaListComponent]
    });
    fixture = TestBed.createComponent(TipotarefaListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
