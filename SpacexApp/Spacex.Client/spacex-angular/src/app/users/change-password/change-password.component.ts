import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Result, UpdatePasswordRequest } from '../../models/spacex.models';
import { catchError, mergeMap, of } from 'rxjs';

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent {
  changePasswordForm: FormGroup;
  showPassword = false;
  showConfirmPassword = false;
  showCurrentPassword = false;

    constructor(private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService, private router: Router) {
      this.changePasswordForm = this.fb.nonNullable.group({
        currentPassword: ['', Validators.required],
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

      togglePassword() {
        this.showPassword = !this.showPassword;
      }
    
      toggleConfirmPassword() {
        this.showConfirmPassword = !this.showConfirmPassword;
      }

      toggleCurrentPassword(){
        this.showCurrentPassword = !this.showCurrentPassword;
      }

      onSubmit(){
       if (this.changePasswordForm.invalid) {
             this.changePasswordForm.markAllAsTouched();
             return;
           }
         
           const request: UpdatePasswordRequest = {
            currentPassword: this.changePasswordForm.controls['currentPassword'].value,
             newPassword: this.changePasswordForm.controls['password'].value
           };
         
               this.authService.updatePassword(request).pipe(
                 mergeMap((result: Result<void>) => {
                   if (result.isSuccess) {
                    this.toastr.success('Update password successful! Redirecting...');
                     setTimeout(() => this.router.navigate(['/dashboard/profile']), 2000);
                   } else {
                     throw new Error(result?.message || 'Login failed.');
                   }
                   return of(null);
                 }),
                 catchError((error) => {
                     this.toastr.error(error?.error?.message ?? error?.message ?? "Update password failed.");
                   return of(null);
                 })
               ).subscribe();
      }
}
