import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementsViewComponent } from './measurements-view.component';

describe('MeasurementsViewComponent', () => {
  let component: MeasurementsViewComponent;
  let fixture: ComponentFixture<MeasurementsViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MeasurementsViewComponent]
    });
    fixture = TestBed.createComponent(MeasurementsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
