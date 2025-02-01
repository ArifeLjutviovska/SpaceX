import { Component, Input } from '@angular/core';
import { SpaceXLaunchDto } from '../../../models/spacex.models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-launch-card',
  templateUrl: './launch-card.component.html',
  styleUrl: './launch-card.component.scss',
  imports: [CommonModule]
})
export class LaunchCardComponent {
  @Input() launch!: SpaceXLaunchDto;
}
