import { Routes } from '@angular/router';
import { SignupComponent } from './users/signup-component/signup.component';
import { DashboardComponent } from './dashboard-component/dashboard.component';
import { LoginComponent } from './users/login-component/login.component';
import { AuthGuard } from './guards/auth.guard';
import { LatestLaunchesComponent } from './spacex-launches/latest-launches/latest-launches.component';
import { UpcomingLaunchesComponent } from './spacex-launches/upcoming-launches/upcoming-launches.component';
import { PastLaunchesComponent } from './spacex-launches/past-launches/past-launches.component';
import { ProfileComponent } from './users/profile-component/profile.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ResetPasswordComponent } from './users/reset-password/reset-password.component';
import { ResetPasswordGuard } from './guards/reset-password.guard';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, 
  { path: 'signup', component: SignupComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'change-password', component: ChangePasswordComponent, canActivate: [AuthGuard] }, 
  { path: 'reset-password', component: ResetPasswordComponent, canActivate: [ResetPasswordGuard] }, 
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard], 
    children: [
      { path: 'profile', component: ProfileComponent },
      { path: 'latest-launches', component: LatestLaunchesComponent },
      { path: 'upcoming-launches', component: UpcomingLaunchesComponent },
      { path: 'past-launches', component: PastLaunchesComponent },
      { path: '', redirectTo: 'profile', pathMatch: 'full' }
    ]
  },
];
