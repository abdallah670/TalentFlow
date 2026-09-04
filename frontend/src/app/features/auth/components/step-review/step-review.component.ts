import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateRegistrationService } from '@features/auth/services/registration.service';

@Component({
  selector: 'app-step-review',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-review.component.html',
  styleUrl: './step-review.component.scss'
})
export class StepReviewComponent {
  readonly registrationService = inject(CandidateRegistrationService);

  get profile() {
    return this.registrationService.profile();
  }

  get skills() {
    return this.profile.skills || [];
  }

  formatDisplayValue(value: any): string {
    if (value === null || value === undefined || value === '') return 'Not provided';
    if (typeof value === 'boolean') return value ? 'Yes' : 'No';
    return String(value);
  }

  get experienceLabel(): string {
    const years = this.profile.totalYearsOfExperience;
    if (!years && years !== 0) return 'Not provided';
    return `${years} year${years !== 1 ? 's' : ''}`;
  }

  get salaryLabel(): string {
    const min = this.profile.salaryExpectationMin;
    const max = this.profile.salaryExpectationMax;
    const currency = this.profile.currency || '';
    if (!min && !max) return 'Not provided';
    if (min && max) return `${currency} ${min} - ${max}`;
    if (min) return `${currency} ${min}+`;
    return `${currency} Up to ${max}`;
  }
}
