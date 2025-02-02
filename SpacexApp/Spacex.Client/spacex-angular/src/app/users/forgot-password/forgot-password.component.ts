import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { Result, ResultType } from '../../models/spacex.models';
import { catchError, mergeMap, of } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
  imports: [ReactiveFormsModule, CommonModule]
})
export class ForgotPasswordComponent {
  emailError: string = '';
  forgotPasswordForm: FormGroup;

  constructor(
    private authService: AuthService,
    private dialogRef: MatDialogRef<ForgotPasswordComponent>,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {
        this.forgotPasswordForm = this.fb.nonNullable.group({
          email: ['', [Validators.required, Validators.email]]
        }); 
  }

  sendResetLink() {
    if (this.forgotPasswordForm.invalid){
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }

        this.authService.forgotPassword(this.forgotPasswordForm.controls['email'].value).pipe(
          mergeMap((result: Result<void>) => {
            if (result.isSuccess) {
              this.toastr.info("Email validation success. You will be redirected to the reset password page.");
              this.dialogRef.close(this.forgotPasswordForm.controls['email'].value);
            } else {
              throw new Error(result?.message || 'Login failed.');
            }
            return of(null);
          }),
          catchError((error) => {
            this.toastr.error(error?.error?.message ?? error?.message ?? "The email you entered was not found");
            return of(null);
          })
        ).subscribe();
  }

  closeDialog() {
    this.forgotPasswordForm.controls['email'].markAsUntouched();
    this.dialogRef.close();
  }
}
