import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.services';
import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const JwtInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);
  const toastr = inject(ToastrService);
  let isRefreshing = false;

  const token = authService.getAccessToken();

  if (req.url.includes('/login') || req.url.includes('/refresh-token')) {
    return next(req);
  }

  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(req).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401 && !isRefreshing) {
        isRefreshing = true;

        return authService.refreshToken().pipe(
          switchMap((result) => {
            if (result.isSuccess) {
              req = req.clone({
                setHeaders: { Authorization: `Bearer ${result.value.accessToken}` }
              });
              return next(req);
            } else {
              authService.logout();
              toastr.error('Session expired. Please log in again.', 'Error');
              return throwError(() => new Error("Session expired. Please log in again."));
            }
          }),
          catchError(() => {
            authService.logout();
            toastr.error('Session expired. Please log in again.', 'Error');
            return throwError(() => new Error("Session expired. Please log in again."));
          })
        );
      }

      return throwError(() => error);
    })
  );
};