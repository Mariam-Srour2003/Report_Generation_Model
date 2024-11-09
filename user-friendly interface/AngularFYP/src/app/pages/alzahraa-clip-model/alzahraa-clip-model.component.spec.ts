import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlzahraaClipModelComponent } from './alzahraa-clip-model.component';

describe('AlzahraaClipModelComponent', () => {
  let component: AlzahraaClipModelComponent;
  let fixture: ComponentFixture<AlzahraaClipModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AlzahraaClipModelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AlzahraaClipModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
