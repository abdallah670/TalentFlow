import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { NgClass } from '@angular/common';
import { LayoutComponent } from '../../../../components/shared/empolyer-layout/layout.component';
import { EmployerRegistrationService } from '../../../../../core/services/employer-registration.service';


export type Plan = 'free' | 'pro' | 'enterprise';

@Component({
  selector: 'app-subscription',
  standalone: true,
  imports: [LayoutComponent, NgClass],
  templateUrl: './subscription.component.html',
  styleUrl: './subscription.component.scss',
})
export class SubscriptionComponent implements OnInit {
  private router = inject(Router);
  private employerService = inject(EmployerRegistrationService);

  selectedPlan: Plan = 'pro';

  ngOnInit() {
    const plan = this.employerService.profile().selectedPlan as Plan;
    if (plan) {
      this.selectedPlan = plan;
    }
  }

  selectPlan(plan: Plan) {
    this.selectedPlan = plan;
    this.employerService.updateProfile({ selectedPlan: plan });
    this.router.navigate(['/register/review']);
  }

  goBack() {
    this.router.navigate(['/register/workspace']);
  }
}
