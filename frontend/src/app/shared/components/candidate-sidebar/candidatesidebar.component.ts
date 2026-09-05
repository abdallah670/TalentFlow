import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateRegistrationService } from '@features/auth/services/registration.service';


@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './candidatesidebar.component.html',
  styleUrl: './candidatesidebar.component.scss'
})
export class SidebarComponent {
  registrationService = inject(CandidateRegistrationService);
  steps = this.registrationService.steps;
  currentStep = this.registrationService.currentStep;

  /**
   * A step is clickable when it is a previous (already visited) step or
   * when every step before it is valid — i.e. the user cannot jump ahead
   * past the first step with an invalid field.
   */
  canGoTo(stepId: number): boolean {
    if (stepId <= this.currentStep()) {
      return true;
    }
    for (let i = 1; i < stepId; i++) {
      if (!this.registrationService.isStepValid(i)) {
        return false;
      }
    }
    return true;
  }

  goTo(stepId: number): void {
    if (!this.canGoTo(stepId)) {
      return;
    }
    this.registrationService.setStep(stepId);
  }
}

