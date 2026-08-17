import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-setup-account',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './setup-account.component.html',
  styleUrl: './setup-account.component.scss',
})
export class SetupAccountComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  showPassword = false;
  showConfirmPassword = false;

  email = signal('');
  firstName = signal('');
  lastName = signal('');
  role = signal('');
  token = signal('');

  setupForm = this.fb.nonNullable.group({
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
  });

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['email']) this.email.set(params['email']);
      if (params['firstName']) this.firstName.set(params['firstName']);
      if (params['lastName']) this.lastName.set(params['lastName']);
      if (params['role']) this.role.set(params['role']);
      if (params['token']) this.token.set(params['token']);
    });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  isPasswordValid(criterion: 'length' | 'uppercase' | 'special'): boolean {
    const password = this.setupForm.get('password')?.value || '';
    switch (criterion) {
      case 'length':
        return password.length >= 8;
      case 'uppercase':
        return /[A-Z]/.test(password);
      case 'special':
        return /[^A-Za-z0-9]/.test(password);
      default:
        return false;
    }
  }

  onSubmit(): void {
    if (this.setupForm.invalid || this.setupForm.value.password !== this.setupForm.value.confirmPassword) {
      this.setupForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.authService.acceptInvitation({
      token: this.token(),
      password: this.setupForm.value.password || '',
      confirmPassword: this.setupForm.value.confirmPassword || '',
    }).subscribe({
      next: () => {
        this.isLoading.set(false);
        Swal.fire({
          icon: 'success',
          title: 'Welcome!',
          text: 'Your account has been set up successfully.',
          timer: 3000,
          showConfirmButton: false,
          position: 'top-end',
          toast: true,
        });
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || 'Failed to set up account. The invitation may have expired.');
      },
    });
  }
}
