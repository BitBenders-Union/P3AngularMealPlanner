import { TestBed } from '@angular/core/testing';

import { UserRecipeScheduleService } from './user-recipe-schedule.service';

describe('UserRecipeScheduleService', () => {
  let service: UserRecipeScheduleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserRecipeScheduleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
