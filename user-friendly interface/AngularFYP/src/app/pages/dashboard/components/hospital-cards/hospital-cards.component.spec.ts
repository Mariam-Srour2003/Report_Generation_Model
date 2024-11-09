import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HospitalCardsComponent } from './hospital-cards.component';

describe('HospitalCardsComponent', () => {
  let component: HospitalCardsComponent;
  let fixture: ComponentFixture<HospitalCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HospitalCardsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HospitalCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
