import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: { firstName: string; lastName: string; email: string } | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.user = this.authService.getCurrentUser();
  }

  navigateToChangePassword() {
    this.router.navigate(['/change-password']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
