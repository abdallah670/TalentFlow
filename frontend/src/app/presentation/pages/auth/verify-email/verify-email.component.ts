import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './verify-email.component.html',
  styleUrl: './verify-email.component.scss',
})
export class VerifyEmailComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  email = signal('');
  isLoading = signal(false);
  isVerified = signal(false);
  errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['email']) {
        this.email.set(params['email']);
      }
      if (params['token']) {
        this.verifyEmail(params['token']);
      }
    });
  }

  verifyEmail(token: string): void {
    if (!this.email()) return;
    
    this.isLoading.set(true);
    this.authService.verifyEmail({ email: this.email(), token }).subscribe({
      next: () => {
        this.isVerified.set(true);
        this.isLoading.set(false);
        Swal.fire({
          icon: 'success',
          title: 'Email Verified!',
          text: 'Your email has been successfully verified.',
          timer: 3000,
          showConfirmButton: false,
          position: 'top-end',
          toast: true,
        });
        setTimeout(() => this.router.navigate(['/']), 2000);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || 'Verification failed. The link may have expired.');
      },
    });
  }

  resendVerification(): void {
    if (!this.email()) return;
    
    this.isLoading.set(true);
    this.authService.resendVerificationEmail({ email: this.email() }).subscribe({
      next: () => {
        this.isLoading.set(false);
        Swal.fire({
          icon: 'success',
          title: 'Email Sent!',
          text: 'A new verification link has been sent to your email.',
          timer: 3000,
          showConfirmButton: false,
          position: 'top-end',
          toast: true,
        });
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || 'Failed to resend verification email.');
      },
    });
  }

  changeEmail(): void {
    this.router.navigate(['/register']);
  }
}
