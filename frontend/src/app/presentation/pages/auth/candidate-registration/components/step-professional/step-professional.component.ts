import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CandidateRegistrationService } from '../../../../../../core/services/registration.service';

@Component({
  selector: 'app-step-professional',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-professional.component.html',
  styleUrl: './step-professional.component.scss'
})
export class StepProfessionalComponent {
  private readonly fb = inject(FormBuilder);
  readonly registrationService = inject(CandidateRegistrationService);

  readonly form = this.fb.nonNullable.group({
    phoneNumber: [''],
    currentJobTitle: [''],
    currentCompany: [''],
    totalYearsOfExperience: [0, [Validators.min(0), Validators.max(50)]],
    linkedinUrl: ['', [Validators.pattern('https?://.*')]],
    portfolioUrl: ['', [Validators.pattern('https?://.*')]],
  });

  constructor() {
    // Load existing values from profile signal
    const profile = this.registrationService.profile();
    this.form.patchValue({
      phoneNumber: profile.phoneNumber || '',
      currentJobTitle: profile.currentJobTitle || '',
      currentCompany: profile.currentCompany || '',
      totalYearsOfExperience: profile.totalYearsOfExperience || 0,
      linkedinUrl: profile.linkedinUrl || '',
      portfolioUrl: profile.portfolioUrl || '',
    });

    // Sync form changes to profile signal
    this.form.valueChanges.subscribe(() => {
      const value = this.form.getRawValue();
      this.registrationService.updateProfile({
        phoneNumber: value.phoneNumber,
        currentJobTitle: value.currentJobTitle,
        currentCompany: value.currentCompany,
        totalYearsOfExperience: value.totalYearsOfExperience as number,
        linkedinUrl: value.linkedinUrl,
        portfolioUrl: value.portfolioUrl,
      });
    });
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && (control.touched || control.dirty);
  }
}