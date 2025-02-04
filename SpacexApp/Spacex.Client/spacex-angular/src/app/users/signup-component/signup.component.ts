import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import {  Result, SignUpRequest } from '../../models/spacex.models';
import { AuthService  } from '../../services/auth.service';
import { catchError, mergeMap, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss',
  imports: [CommonModule, ReactiveFormsModule]
})
export class SignupComponent {
  signupForm: FormGroup;
  showPassword = false;
  showConfirmPassword = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService, private router: Router) {
    this.signupForm = this.fb.nonNullable.group({
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(75)]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(50),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[.@$!%*?&])[A-Za-z\d.@$!%*?&]{8,}$/)
      ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator() }); 
  }

  private passwordMatchValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      const password = control.get('password')?.value;
      const confirmPassword = control.get('confirmPassword')?.value;
      return password === confirmPassword ? null : { mismatch: true };
    };
  }

  onLoginClicked(){
    this.router.navigate(["/login"]);
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  showSuccess(message: string) {
    this.toastr.success(message);
  }

  showError(message: string) {
    this.toastr.error(message);
  }

  onSubmit() {
    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }
  
    const request: SignUpRequest = {
      firstName: this.signupForm.controls['firstName'].value,
      lastName: this.signupForm.controls['lastName'].value,
      email: this.signupForm.controls['email'].value,
      password: this.signupForm.controls['password'].value
    };
  
    this.authService.signUp(request).pipe(
      mergeMap((result: Result<void>) => {
        if (result.isSuccess) {
          this.showSuccess('Signup successful! Redirecting...');
          return this.authService.login({ email: request.email, password: request.password });
        } else {
          this.showError(result?.message || 'Signup failed.');
          throw new Error(result?.message || 'Signup failed.');
        }
      }),
      mergeMap((loginResult: Result<void>) => {
        if (loginResult.isSuccess) {
          localStorage.setItem("isAuthenticated", "true"); 
          setTimeout(() => this.router.navigate(['/dashboard/profile']), 2000);
        } else {
          this.showError(loginResult?.message || 'Authenticate user failed.');
        }
        return of(null);
      }),
      catchError((error) => {
        this.showError(error?.message ?? "Authenticate user failed.");
        return of(null);
      })
    ).subscribe();
  }
  
}
