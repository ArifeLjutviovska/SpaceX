import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CurrentUserResponse } from '../../models/spacex.models';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: CurrentUserResponse | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.authService.getCurrentUser().subscribe(user => {
      if (user) {
        this.user = {
          firstName: user.value.firstName ?? '',
          lastName: user.value.lastName ?? '',
          email: user.value.email ?? ''
        };
      } else {
        this.user = null;
      }
    });
  }

  navigateToChangePassword() {
    this.router.navigate(['/change-password']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
