import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-step2',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './step2.component.html',
  styleUrl: './step2.component.scss',
})
export class Step2Component {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  registerForm = this.fb.nonNullable.group({
    phoneNumber: [''],
    currentJobTitle: [''],
    currentCompany: [''],
    totalYearsOfExperience: ['', [Validators.min(0), Validators.max(50)]],
    linkedinUrl: ['', [Validators.pattern('https?://.*')]],
    portfolioUrl: ['', [Validators.pattern('https?://.*')]],
  });

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const data = this.registerForm.getRawValue();
    this.authService.registerCandidate(data as any).subscribe({
      next: () => {
        this.router.navigate(['/verify-email']);
      },
      error: (err) => {
        console.error('Registration failed:', err);
      },
    });
  }

  onBack(): void {
    this.router.navigate(['step1'], { relativeTo: this.route });
  }
}
