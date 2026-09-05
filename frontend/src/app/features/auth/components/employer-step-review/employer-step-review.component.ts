import { Component, inject, OnInit } from '@angular/core';

import { TitleCasePipe } from '@angular/common';
import { EmployerRegistrationService } from '../../services/employer-registration.service';
import { Plan } from '../employer-step-subscription/employer-step-subscription.component';

@Component({
  selector: 'app-employer-step-review',
  standalone: true,
  imports: [TitleCasePipe],
  templateUrl: './employer-step-review.component.html',
  styleUrl: './employer-step-review.component.scss',
})
export class EmployerStepReviewComponent implements OnInit {
  private employerService = inject(EmployerRegistrationService);

  selectedPlan: Plan = 'pro';

  ngOnInit() {
    const plan = this.employerService.profile().selectedPlan as Plan;
    if (plan) {
      this.selectedPlan = plan;
    }
  }

  get profile() {
    return this.employerService.profile();
  }

  getPlanPrice(plan: Plan): string {
    switch (plan) {
      case 'free': return '$0';
      case 'pro': return '$49';
      case 'enterprise': return 'Custom';
      default: return '';
    }
  }

  editStep(stepId: number) {
    this.employerService.setStep(stepId);
  }
}