import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { Result, SignUpRequest, LoginRequest, LoginResponse,  UpdatePasswordRequest, ResetPasswordRequest } from "../models/spacex.models";
import { ToastrService } from "ngx-toastr";
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl: string = "http://localhost:7005/api/auth";
  private jwtHelper = new JwtHelperService();

  constructor(private httpService: HttpClient, private toastr: ToastrService){}

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

  updatePassword(data: UpdatePasswordRequest): Observable<Result<void>> {
    return this.httpService.put<Result<void>>(`${this.baseApiUrl}/update-password`, data);
  }
  
  resetPassword(data: ResetPasswordRequest): Observable<Result<void>> {
    return this.httpService.put<Result<void>>(`${this.baseApiUrl}/reset-password`, data);
  }

  getCurrentUser(): { firstName: string; lastName: string; email: string } | null {
    const token = this.getAccessToken();
    if (!token || this.jwtHelper.isTokenExpired(token)) {
      return null; 
    }
    try {
      const decodedToken: any = this.jwtHelper.decodeToken(token);
      return {
        firstName: decodedToken.firstName || '',
        lastName: decodedToken.lastName || '',
        email: decodedToken.email || '',
      };
    } catch (error) {
      console.error('Error decoding token', error);
      this.toastr.error('Session expired or invalid token. Please log in again.', 'Error');
      return null;
    }
  }

  public logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

  forgotPassword(email: string) : Observable<Result<void>>{
    return this.httpService.put<Result<void>>(`${this.baseApiUrl}/forgot-password`, {email: email});
  }
  
  public getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }
}
