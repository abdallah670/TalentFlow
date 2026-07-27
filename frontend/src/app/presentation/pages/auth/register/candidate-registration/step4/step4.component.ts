import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-step4',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './step4.component.html',
  styleUrl: './step4.component.scss',
})
export class Step4Component {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  registerForm = this.fb.nonNullable.group({
    preferredLocation: [''],
    remoteOnly: [false],
    salaryExpectationMin: ['', [Validators.min(0)]],
    salaryExpectationMax: ['', [Validators.min(0)]],
    currency: ['USD'],
    availableFrom: [''],
    workAuthorization: [''],
  });

  onBack(): void {
    this.router.navigate(['step3'], { relativeTo: this.route });
  }

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
}
