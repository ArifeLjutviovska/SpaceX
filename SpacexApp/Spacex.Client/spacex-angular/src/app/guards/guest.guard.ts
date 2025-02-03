import { Injectable, inject } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GuestGuard implements CanActivate {
  private router = inject(Router);

  canActivate(): Observable<boolean> {
    const isAuthenticated = localStorage.getItem("isAuthenticated") === "true";

    if (isAuthenticated) {
      this.router.navigate(['/dashboard/profile']);
      return of(false);
    }
    return of(true);
  }
}