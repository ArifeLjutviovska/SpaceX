import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, map, Observable, of, tap, throwError } from "rxjs";
import { Result, SignUpRequest, LoginRequest,  UpdatePasswordRequest, ResetPasswordRequest, CurrentUserResponse } from "../models/spacex.models";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl: string = "http://localhost:7005/api/auth";

  constructor(private httpService: HttpClient, private toastr: ToastrService, private router: Router){}

  public signUp(user: SignUpRequest): Observable<Result<void>> {
    return this.httpService.post<Result<void>>(`${this.baseApiUrl}/signup`, user, { withCredentials: true });
  }
  public login(user: LoginRequest): Observable<Result<void>> {
    return this.httpService.post<Result<void>>(`${this.baseApiUrl}/login`, user, { withCredentials: true });
  }
 
  public refreshToken(): Observable<Result<void>> {
    return this.httpService.post<Result<void>>(`${this.baseApiUrl}/refresh-token`, {}, { withCredentials: true });
  }

  updatePassword(data: UpdatePasswordRequest): Observable<Result<void>> {
    return this.httpService.put<Result<void>>(`${this.baseApiUrl}/update-password`, data, { withCredentials: true });
  }
  
  resetPassword(data: ResetPasswordRequest): Observable<Result<void>> {
    return this.httpService.put<Result<void>>(`${this.baseApiUrl}/reset-password`, data, { withCredentials: true });
  }

  public getCurrentUser(): Observable<Result<CurrentUserResponse>> {
    return this.httpService.get<Result<CurrentUserResponse>>(
      `${this.baseApiUrl}/current-user`,
      { withCredentials: true }
    );
  }

  public verifySession(): Observable<boolean> {
    const isAuthenticated = localStorage.getItem("isAuthenticated") === "true";
    if (isAuthenticated) return of(true);
    return this.httpService.get<Result<void>>(`${this.baseApiUrl}/verify-session`, { withCredentials: true })
      .pipe(
        map(result => {
          if (result.isSuccess) {
            localStorage.setItem("isAuthenticated", "true");
            return true;
          } else {
            localStorage.removeItem("isAuthenticated");
            return false;
          }
        }),
        catchError(() => {
          localStorage.removeItem("isAuthenticated");
          return of(false);
        })
      );
  }
  
  public logout(): void {
    this.httpService.post(`${this.baseApiUrl}/logout`, {}, { withCredentials: true }).subscribe({
      next: () => {
        localStorage.removeItem("isAuthenticated");  
        document.cookie = "accessToken=; Max-Age=0; path=/;";
        document.cookie = "refreshToken=; Max-Age=0; path=/;";
        if (!window.location.href.includes("/login") && !window.location.href.includes("/signup")){
          this.toastr.info("Successfully logged out. Redirecting to login page.");
        }
        this.router.navigate(['/login']);
      },
      error: () => {
        localStorage.removeItem("isAuthenticated");  
        this.router.navigate(['/login']);
      }
    });
  }

  forgotPassword(email: string) : Observable<Result<void>>{
    return this.httpService.put<Result<void>>(`${this.baseApiUrl}/forgot-password`, {email: email}, { withCredentials: true });
  }
}