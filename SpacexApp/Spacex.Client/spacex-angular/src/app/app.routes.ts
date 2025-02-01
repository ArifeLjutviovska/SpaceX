import { Routes } from '@angular/router';
import { SignupComponent } from './signup-component/signup.component';
import { DashboardComponent } from './dashboard-component/dashboard.component';
import { LoginComponent } from './login-component/login.component';
import { AuthGuard } from './guards/auth.guard';
import { LatestLaunchesComponent } from './spacex-launches/latest-launches/latest-launches.component';
import { UpcomingLaunchesComponent } from './spacex-launches/upcoming-launches/upcoming-launches.component';
import { PastLaunchesComponent } from './spacex-launches/past-launches/past-launches.component';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, 
  { path: 'signup', component: SignupComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard], 
    children: [
      { path: 'profile', component: LatestLaunchesComponent }, // to be updated
      { path: 'latest-launches', component: LatestLaunchesComponent },
      { path: 'upcoming-launches', component: UpcomingLaunchesComponent },
      { path: 'past-launches', component: PastLaunchesComponent },
      { path: '', redirectTo: 'latest-launches', pathMatch: 'full' } 
    ]
  },
];
