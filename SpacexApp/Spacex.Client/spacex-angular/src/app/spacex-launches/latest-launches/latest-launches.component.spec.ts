import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LatestLaunchesComponent } from './latest-launches.component';

describe('LatestLaunchesComponent', () => {
  let component: LatestLaunchesComponent;
  let fixture: ComponentFixture<LatestLaunchesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LatestLaunchesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LatestLaunchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
