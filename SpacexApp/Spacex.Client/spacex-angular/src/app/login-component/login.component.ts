import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { LoginRequest, LoginResponse, Result, SignUpRequest } from '../models/spacex.models';
import { AuthService } from '../services/auth.service';
import { catchError, mergeMap, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService, private router: Router) {
    this.loginForm = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    }); 
  }


  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onForgotPasswordClicked(){
    console.log("Forgot password clicked.");
  }

  onSingUpClicked(){
    this.router.navigate(["/signup"]);
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Success');
  }

  showError(message: string) {
    this.toastr.error(message, 'Error');
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
  
    const request: LoginRequest = {
      email: this.loginForm.controls['email'].value,
      password: this.loginForm.controls['password'].value
    };
  
    this.authService.login(request).pipe(
      mergeMap((result: Result<LoginResponse>) => {
        if (result.isSuccess) {
          this.showSuccess('Login successful! Redirecting...');
          setTimeout(() => this.router.navigate(['/dashboard']), 2000);
        } else {
          throw new Error(result?.message || 'Login failed.');
        }
        return of(null);
      }),
      catchError((error) => {
        if (!error?.message.includes('Session expired')) { 
          this.showError(error?.error?.message ?? error?.message ?? "Login failed.");
        }
        return of(null);
      })
    ).subscribe();
  }
  
}
