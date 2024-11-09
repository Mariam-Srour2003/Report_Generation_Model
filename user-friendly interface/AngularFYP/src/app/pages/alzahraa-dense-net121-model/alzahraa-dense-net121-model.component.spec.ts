import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlzahraaDenseNet121ModelComponent } from './alzahraa-dense-net121-model.component';

describe('AlzahraaDenseNet121ModelComponent', () => {
  let component: AlzahraaDenseNet121ModelComponent;
  let fixture: ComponentFixture<AlzahraaDenseNet121ModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AlzahraaDenseNet121ModelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AlzahraaDenseNet121ModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
