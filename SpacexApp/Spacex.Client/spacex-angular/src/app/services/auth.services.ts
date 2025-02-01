import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { Result, SignUpRequest, LoginRequest, LoginResponse } from "../models/spacex.models";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl: string = "http://localhost:7005/api/auth";

  constructor(private httpService: HttpClient){}

  public signUp(user: SignUpRequest): Observable<Result<void>> {
    return this.httpService.post<Result<void>>(`${this.baseApiUrl}/signup`, user);
  }

  public login(user: LoginRequest): Observable<Result<LoginResponse>> {
    return this.httpService.post<Result<LoginResponse>>(`${this.baseApiUrl}/login`, user).pipe(
      tap(response => {
        if (response.isSuccess) {
          localStorage.setItem('accessToken', response.value.accessToken);
          localStorage.setItem('refreshToken', response.value.refreshToken);
        }
      })
    );
  }

  public refreshToken(): Observable<Result<LoginResponse>> {
    const refreshToken = localStorage.getItem('refreshToken');
    return this.httpService.post<Result<LoginResponse>>(`${this.baseApiUrl}/refresh-token`, { refreshToken }).pipe(
      tap(response => {
        if (response.isSuccess) {
          localStorage.setItem('accessToken', response.value.accessToken);
          localStorage.setItem('refreshToken', response.value.refreshToken);
        }
      })
    );
  }

  public logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

  public getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }
}
