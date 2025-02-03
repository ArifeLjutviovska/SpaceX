import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, switchMap, throwError, of } from 'rxjs';

export const JwtInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);

  req = req.clone({ withCredentials: true });

  return next(req).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        const isAuthRequest = req.url.includes('/refresh-token') || req.url.includes('/verify-session') || req.url.includes('/login');

        if (isAuthRequest) {
          return throwError(() => error);
        }

        if (!document.cookie.includes("refreshToken")) {  
          authService.logout();
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