import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../../environments/environment';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  showPassword = false;

  registerForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    userName: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  getPasswordStrength(): number {
    const password = this.registerForm.get('password')?.value || '';
    if (!password) return 0;
    let strength = 0;
    if (password.length >= 8) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;
    return strength;
  }

  loginWithGoogle(): void {
    this.isLoading.set(true);
    window.location.href = `${environment.baseUrl}/Auth/google`;
  }

  loginWithLinkedIn(): void {
    this.isLoading.set(true);
    window.location.href = `${environment.baseUrl}/Auth/linkedin`;
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.authService.register(this.registerForm.getRawValue()).subscribe({
      next: (response) => {
        Swal.fire({
          icon: 'success',
          title: 'Account Created!',
          text: 'Welcome! Please verify your email to activate your account.',
          timer: 3000,
          showConfirmButton: false,
          position: 'top-end',
          toast: true,
        });
        this.router.navigate(['/verify-email'], { queryParams: { email: response.email || this.registerForm.value.email } });
      },
      error: (err: any) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || err.message || 'Registration failed.');
        Swal.fire({
          icon: 'error',
          title: 'Oops!',
          text: this.errorMessage()!,
          confirmButtonColor: '#e63946',
        });
      },
    });
  }
}
