import { Routes } from '@angular/router';
import { SignupComponent } from './signup-component/signup.component';
import { DashboardComponent } from './dashboard-component/dashboard.component';
import { LoginComponent } from './login-component/login.component';
import { AuthGuard } from './guards/auth.guard';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, 
  { path: 'signup', component: SignupComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
];
