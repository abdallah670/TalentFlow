import { Component, inject, OnInit } from '@angular/core';

import { EmployerRegistrationService } from '../../services/employer-registration.service';

export type Plan = 'free' | 'pro' | 'enterprise';

@Component({
  selector: 'app-employer-step-subscription',
  standalone: true,
  templateUrl: './employer-step-subscription.component.html',
  styleUrl: './employer-step-subscription.component.scss',
})
export class EmployerStepSubscriptionComponent implements OnInit {
  private employerService = inject(EmployerRegistrationService);

  selectedPlan: Plan = 'pro';

  ngOnInit() {
    const plan = this.employerService.profile().selectedPlan as Plan;
    if (plan) {
      this.selectedPlan = plan;
    }
  }

  isSelected(plan: Plan): boolean {
    return this.selectedPlan === plan;
  }

  selectPlan(plan: Plan) {
    this.selectedPlan = plan;
    this.employerService.updateProfile({ selectedPlan: plan });
    this.employerService.setStepValid(3, true);
  }
}