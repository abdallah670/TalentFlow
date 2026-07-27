import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-step3',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './step3.component.html',
  styleUrl: './step3.component.scss',
})
export class Step3Component {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  registerForm = this.fb.nonNullable.group({
    skills: [''],
    coverLetter: [''],
  });

  onBack(): void {
    this.router.navigate(['step2'], { relativeTo: this.route });
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
