import { Routes } from '@angular/router';
import { SignupComponent } from './signup-component/signup.component';
import { DashboardComponent } from './dashboard-component/dashboard.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'signup', pathMatch: 'full' }, 
  { path: 'signup', component: SignupComponent },
  { path: 'dashboard', component: DashboardComponent },
];
