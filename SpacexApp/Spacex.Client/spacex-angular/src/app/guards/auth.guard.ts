import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const token = this.authService.getAccessToken();
    const isAuthRoute = route.routeConfig?.path === 'login' || route.routeConfig?.path === 'signup';

    if (token) {
      if (isAuthRoute) {
        this.toastr.info("You are already logged in.", 'Info');
        this.router.navigate(['/dashboard']); 
        return false;
      }
      return true;
    } else {
      if (!isAuthRoute) {
        this.toastr.error("Please log in first!");
        this.router.navigate(['/login']); 
        return false;
      }
      return true; 
    }
  }
}
