import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployerRegistrationService } from '@features/auth/services/employer-registration.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './empolyersidebar.component.html',
  styleUrl: './empolyersidebar.component.scss',
})
export class SidebarComponent {
  employerService = inject(EmployerRegistrationService);
  steps = this.employerService.steps;
  currentStep = this.employerService.currentStep;

  private readonly icons = ['business', 'domain', 'payments', 'fact_check'];

  /** A step is clickable when it is a previous step or when every step
   *  before it is valid — i.e. the user cannot jump ahead past an
   *  invalid/blank step. */
  canGoTo(stepId: number): boolean {
    if (stepId <= this.currentStep()) {
      return true;
    }
    for (let i = 1; i < stepId; i++) {
      if (!this.employerService.isStepValid(i)) {
        return false;
      }
    }
    return true;
  }

  goTo(stepId: number): void {
    if (this.canGoTo(stepId)) {
      this.employerService.setStep(stepId);
    }
  }

  iconFor(stepId: number): string {
    return this.icons[stepId - 1] ?? 'circle';
  }
}

