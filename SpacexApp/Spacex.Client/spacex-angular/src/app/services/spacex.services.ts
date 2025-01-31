import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Result } from "../models/spacex.models";

export interface SignUpRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class SpacexService {

  private baseApiUrl: string = "http://localhost:7005/api/auth";

  constructor(private httpService: HttpClient){}

  public signUp(user: SignUpRequest): Observable<Result<void>> {
    return this.httpService.post<Result<void>>(`${this.baseApiUrl}/signup`, user);
  }
}
