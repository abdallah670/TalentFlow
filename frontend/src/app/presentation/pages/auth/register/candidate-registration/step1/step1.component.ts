import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-step1',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './step1.component.html',
  styleUrl: './step1.component.scss',
})
export class Step1Component {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isLoading = signal(false);
  showPassword = signal(false);
  showConfirmPassword = signal(false);

  registerForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
    privacyAccepted: [false, Validators.requiredTrue],
  });

  togglePassword(): void {
    this.showPassword.update((value: boolean) => !value);
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword.update((value: boolean) => !value);
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const { confirmPassword, privacyAccepted, ...request } = this.registerForm.getRawValue();
    this.isLoading.set(true);
    this.authService.registerCandidate(request as any).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        this.router.navigate(['/verify-email'], { queryParams: { email: response.email } });
      },
      error: (err) => {
        this.isLoading.set(false);
        console.error('Registration failed:', err);
      },
    });
  }
}
