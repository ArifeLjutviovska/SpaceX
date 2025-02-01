import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result, PagedResult, SpaceXLaunchDto } from '../models/spacex.models';

@Injectable({
  providedIn: 'root'
})
export class SpacexService {
  private baseApiUrl = 'http://localhost:7005/api/spacex';

  constructor(private httpService: HttpClient) {}

  getPastLaunches(pageNumber: number, pageSize: number): Observable<Result<PagedResult<SpaceXLaunchDto>>> {
    return this.httpService.get<Result<PagedResult<SpaceXLaunchDto>>>(
      `${this.baseApiUrl}/past-launches?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  getLatestLaunches(pageNumber: number, pageSize: number): Observable<Result<PagedResult<SpaceXLaunchDto>>> {
    return this.httpService.get<Result<PagedResult<SpaceXLaunchDto>>>(
      `${this.baseApiUrl}/latest-launches?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  getUpcomingLaunches(pageNumber: number, pageSize: number): Observable<Result<PagedResult<SpaceXLaunchDto>>> {
    return this.httpService.get<Result<PagedResult<SpaceXLaunchDto>>>(
      `${this.baseApiUrl}/upcoming-launches?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }
}
