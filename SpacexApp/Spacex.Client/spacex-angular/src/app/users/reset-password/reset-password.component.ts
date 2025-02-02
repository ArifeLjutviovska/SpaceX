import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPasswordRequest, Result} from '../../models/spacex.models';
import { catchError, mergeMap, of } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit{
  changePasswordForm: FormGroup;
  showPassword = false;
  showConfirmPassword = false;
  email = '';

    constructor(private fb: FormBuilder, 
                private authService: AuthService, 
                private toastr: ToastrService, 
                private router: Router,
                private route: ActivatedRoute) 
    {
      this.changePasswordForm = this.fb.nonNullable.group({
        password: ['', [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(50),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[.@$!%*?&])[A-Za-z\d.@$!%*?&]{8,}$/)
        ]],
        confirmPassword: ['', Validators.required]
      }, { validators: this.passwordMatchValidator() }); 
    }

    ngOnInit(): void {
      this.route.queryParams.subscribe(params => {
        this.email = params['email'] || '';
    
        if (!this.email) {
          this.toastr.error("Invalid access. Please request a reset again.");
          this.router.navigate(['/login']);
        }
      });
    }

      togglePassword() {
        this.showPassword = !this.showPassword;
      }
    
      toggleConfirmPassword() {
        this.showConfirmPassword = !this.showConfirmPassword;
      }

      onSubmit(){
       if (this.changePasswordForm.invalid) {
             this.changePasswordForm.markAllAsTouched();
             return;
           }

           if (!this.email) {
             this.toastr.error("Invalid access. Please request a reset again.");
             this.router.navigate(['/login']); 
           }
         
           const request: ResetPasswordRequest = {
             email: this.email,
             newPassword: this.changePasswordForm.controls['password'].value
           };
         
               this.authService.resetPassword(request).pipe(
                 mergeMap((result: Result<void>) => {
                   if (result.isSuccess) {
                    this.toastr.success('The password was reset successfully! Redirecting to the login page...');
                     setTimeout(() => this.router.navigate(['/login']), 2000);
                   } else {
                     throw new Error(result?.message || 'Password reset failed.');
                   }
                   return of(null);
                 }),
                 catchError((error) => {
                     this.toastr.error(error?.error?.message ?? error?.message ?? "Reset password failed.");
                   return of(null);
                 })
               ).subscribe();
      }

      private passwordMatchValidator(): ValidatorFn {
        return (control: AbstractControl) => {
          const password = control.get('password')?.value;
          const confirmPassword = control.get('confirmPassword')?.value;
          return password === confirmPassword ? null : { mismatch: true };
        };
      }
}
