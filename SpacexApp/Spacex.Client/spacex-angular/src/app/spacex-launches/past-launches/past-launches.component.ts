import { Component, OnInit } from '@angular/core';
import { SpacexService } from '../../services/spacex.service';
import { SpaceXLaunchDto } from '../../models/spacex.models';
import { LaunchCardComponent } from '../common/launch-card/launch-card.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-past-launches',
  templateUrl: './past-launches.component.html',
  styleUrl: './past-launches.component.scss',
  imports: [LaunchCardComponent, CommonModule, FormsModule]
})
export class PastLaunchesComponent implements OnInit {
  launches: SpaceXLaunchDto[] = [];
  currentPage = 1;
  totalPages = 0;
  pageSize = 20;
  totalItems = 0;
  launchesLoaded = false;

  constructor(private spacexService: SpacexService) {}

  ngOnInit() {
    this.loadLaunches();
  }

  loadLaunches() {
    this.spacexService.getPastLaunches(this.currentPage, this.pageSize).subscribe(response => {
      if (response.isSuccess && response.value) {
        this.launchesLoaded = true;
        this.launches = response.value.items;
        this.totalPages = response.value.totalPages || 0;
        this.currentPage = response.value.currentPage || 1;
        this.totalItems = response.value.totalItems || 0;
      } else {
        this.launchesLoaded = true;
        this.launches = [];
        this.totalPages = 0;  
        this.currentPage = 1;
        this.totalItems = 0;
      }
    });

  }
  

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadLaunches();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadLaunches();
    }
  }

  changePageSize(event: Event) {
    this.pageSize = Number((event.target as HTMLSelectElement).value);
    this.currentPage = 1; 
    this.loadLaunches();
  }
}
