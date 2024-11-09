import { TestBed } from '@angular/core/testing';

import { ExcelDataService } from './excel-data.service';

describe('ExcelDataService', () => {
  let service: ExcelDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExcelDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
