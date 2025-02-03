import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, switchMap, throwError, of } from 'rxjs';
import { Router } from '@angular/router';
import {  ToastrService } from 'ngx-toastr';

export const JwtInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);
  const router = inject(Router)
  const toastr = inject(ToastrService)

  req = req.clone({ withCredentials: true });

  return next(req).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        const isAuthRequest = req.url.includes('/refresh-token') || req.url.includes('/verify-session') || req.url.includes('/login') || req.url.includes('/logout');

        if (isAuthRequest) {
          return throwError(() => error);
        }

        const isAuthenticated = localStorage.getItem("isAuthenticated") === "true";
          if (!document.cookie.includes("refreshToken") || !isAuthenticated) {  
            localStorage.removeItem("isAuthenticated");
            document.cookie = "accessToken=; Max-Age=0; path=/;";
            document.cookie = "refreshToken=; Max-Age=0; path=/;";
            toastr.info("Your session has expired. Please log in again.");
            setTimeout(() => router.navigate(['/login']), 3000);
            return of();
          }

        return authService.refreshToken().pipe(
          switchMap((result) => {
            if (result.isSuccess) {
              return next(req);
            } else {
              authService.logout();
              return throwError(() => error);
            }
          }),
          catchError(() => {
            authService.logout();
            return throwError(() => error);
          })
        );
      }
      return throwError(() => error);
    })
  );
};