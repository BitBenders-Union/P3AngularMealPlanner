import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartpageComponent } from './startpage.component';

describe('StartpageComponent', () => {
  let component: StartpageComponent;
  let fixture: ComponentFixture<StartpageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StartpageComponent]
    });
    fixture = TestBed.createComponent(StartpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
