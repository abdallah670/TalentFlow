import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CandidateRegistrationService } from '@features/auth/services/registration.service';

@Component({
  selector: 'app-step-personal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-personal.component.html',
  styleUrl: './step-personal.component.scss'
})
export class StepPersonalComponent {
  private readonly fb = inject(FormBuilder);
  readonly registrationService = inject(CandidateRegistrationService);

  showPassword = false;
  showConfirmPassword = false;

  readonly form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), this.passwordComplexityValidator]],
    confirmPassword: ['', [Validators.required]],
    agreeToTerms: [false, [Validators.requiredTrue]],
  }, { validators: this.passwordMatchValidator });

  constructor() {
    // Load existing values from profile signal
    const profile = this.registrationService.profile();
    this.form.patchValue({
      firstName: profile.firstName,
      lastName: profile.lastName,
      email: profile.email,
      password: profile.password,
      confirmPassword: profile.confirmPassword,
      agreeToTerms: false,
    });

    // Sync form changes to profile signal + step validity
    this.form.valueChanges.subscribe(() => {
      const value = this.form.getRawValue();
      this.registrationService.updateProfile({
        firstName: value.firstName,
        lastName: value.lastName,
        email: value.email,
        password: value.password,
        confirmPassword: value.confirmPassword,
      });
      this.registrationService.setStepValid(1, this.form.valid);
    });

    // Set initial validity
    this.registrationService.setStepValid(1, this.form.valid);
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  private passwordComplexityValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value as string;
    if (!value) return null;
    const errors: string[] = [];
    if (!/[A-Z]/.test(value)) errors.push('uppercase');
    if (!/[a-z]/.test(value)) errors.push('lowercase');
    if (!/[0-9]/.test(value)) errors.push('number');
    return errors.length ? { complexity: errors } : null;
  }

  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && (control.touched || control.dirty);
  }
}