import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BahmanModelImplementationComponent } from './bahman-model-implementation.component';

describe('BahmanModelImplementationComponent', () => {
  let component: BahmanModelImplementationComponent;
  let fixture: ComponentFixture<BahmanModelImplementationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BahmanModelImplementationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BahmanModelImplementationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
