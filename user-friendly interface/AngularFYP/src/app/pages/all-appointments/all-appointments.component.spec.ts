import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllAppointmentsComponent } from './all-appointments.component';

describe('AllAppointmentsComponent', () => {
  let component: AllAppointmentsComponent;
  let fixture: ComponentFixture<AllAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllAppointmentsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AllAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
