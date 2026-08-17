import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TitleCasePipe } from '@angular/common';
import { LayoutComponent } from '../../../../components/shared/empolyer-layout/layout.component';
import { EmployerRegistrationService } from '../../../../../core/services/employer-registration.service';
import { Plan } from '../subscription/subscription.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-review',
  standalone: true,
  imports: [LayoutComponent, TitleCasePipe],
  templateUrl: './review.component.html',
  styleUrl: './review.component.scss',
})
export class ReviewComponent implements OnInit {
  private router = inject(Router);
  public employerService = inject(EmployerRegistrationService);
  
  selectedPlan: Plan = 'pro';
  
  isSubmitting = false;

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

  editStep(path: string) {
    this.router.navigate([path]);
  }

  goBack() {
    this.router.navigate(['/register/subscription']);
  }

  confirm() {
    this.isSubmitting = true;
    this.employerService.submit().subscribe({
      next: () => {
        this.isSubmitting = false;
        Swal.fire({
          icon: 'success',
          title: 'Workspace Launched!',
          text: 'Your account has been created. Please check your email to verify your account.',
          timer: 3000,
          showConfirmButton: false,
          position: 'top-end',
          toast: true,
        });
        this.router.navigate(['/verify-email'], {
          queryParams: { email: this.profile.email },
        });
      },
      error: (err: any) => {
        this.isSubmitting = false;
        const message = err?.error?.message || err?.message || 'Registration failed. Please try again.';
        Swal.fire({
          icon: 'error',
          title: 'Registration Failed',
          text: message,
          confirmButtonColor: '#e63946',
        });
      },
    });
  }
}
