import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaunchCardComponent } from './launch-card.component';

describe('LaunchCardComponent', () => {
  let component: LaunchCardComponent;
  let fixture: ComponentFixture<LaunchCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaunchCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LaunchCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
