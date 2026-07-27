import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-company-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './company-details.component.html',
  styleUrl: './company-details.component.scss',
})
export class CompanyDetailsComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  registerForm = this.fb.nonNullable.group({
    companyName: ['', [Validators.required]],
    companySize: ['', [Validators.required]],
    industry: ['', [Validators.required]],
    websiteUrl: ['', [Validators.pattern('https?://.*')]],
    linkedinUrl: ['', [Validators.pattern('https?://.*')]],
    officeLocation: ['', [Validators.required]],
  });

  onBack(): void {
    this.router.navigate(['/register']);
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const data = this.registerForm.getRawValue();
    this.authService.registerEmployer(data as any).subscribe({
      next: () => {
        this.router.navigate(['/verify-email']);
      },
      error: (err) => {
        console.error('Registration failed:', err);
      },
    });
  }
}
