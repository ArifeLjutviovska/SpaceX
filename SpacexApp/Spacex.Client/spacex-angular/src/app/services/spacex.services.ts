import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Result } from "../models/spacex.models";


@Injectable({
    providedIn: 'root'
})
export class SpacexService {

  private baseApiUrl: string = "http://localhost:7005/spacex/api";

  constructor(private httpService: HttpClient){}
  

  public createUser(userName: string): Observable<Result<string>> {
    return this.httpService.post<Result<string>>(
      `${this.baseApiUrl}/create`,
      {
        name: userName
      }
    );
  }
}