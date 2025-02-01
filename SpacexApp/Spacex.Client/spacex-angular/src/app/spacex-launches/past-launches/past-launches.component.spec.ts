import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PastLaunchesComponent } from './past-launches.component';

describe('PastLaunchesComponent', () => {
  let component: PastLaunchesComponent;
  let fixture: ComponentFixture<PastLaunchesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PastLaunchesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PastLaunchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
