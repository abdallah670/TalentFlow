import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-select-plan',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './select-plan.component.html',
  styleUrl: './select-plan.component.scss',
})
export class SelectPlanComponent {
  private readonly router = inject(Router);

  plans = [
    {
      id: 'free',
      name: 'Free',
      price: '$0',
      period: '/month',
      description: 'For startups and small teams.',
      features: [
        'Up to 3 Active Jobs',
        'Basic Candidate Management',
        'Standard Analytics Dashboard',
      ],
      missingFeatures: ['Advanced Reporting'],
      cta: 'Select Free',
      recommended: false,
    },
    {
      id: 'pro',
      name: 'Pro',
      price: '$49',
      period: '/month',
      description: 'For growing companies scaling fast.',
      features: [
        'Unlimited Active Jobs',
        'Advanced Candidate Pipelines',
        'Custom Analytics & Reporting',
        'Automated Email Workflows',
        'Team Collaboration Tools',
      ],
      missingFeatures: [],
      cta: 'Select Pro',
      recommended: true,
    },
    {
      id: 'enterprise',
      name: 'Enterprise',
      price: 'Custom',
      period: '',
      description: 'Tailored solutions for large organizations.',
      features: [
        'Everything in Pro',
        'Single Sign-On (SSO) Integration',
        'Dedicated Account Manager',
        'Custom API Access',
        'SLA Guarantee',
      ],
      missingFeatures: [],
      cta: 'Contact Sales',
      recommended: false,
    },
  ];

  selectPlan(planId: string): void {
    this.router.navigate(['/verify-email'], { queryParams: { plan: planId } });
  }
}
