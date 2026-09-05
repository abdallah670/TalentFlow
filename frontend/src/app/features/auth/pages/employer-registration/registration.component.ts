import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EmployerRegistrationService } from '../../services/employer-registration.service';
import { EmployerStepCompanyComponent } from '../../components/employer-step-company/employer-step-company.component';
import { EmployerStepWorkspaceComponent } from '../../components/employer-step-workspace/employer-step-workspace.component';
import { EmployerStepSubscriptionComponent } from '../../components/employer-step-subscription/employer-step-subscription.component';
import { EmployerStepReviewComponent } from '../../components/employer-step-review/employer-step-review.component';
import { SidebarComponent } from '@shared/components/empolyer-sidebar/empolyersidebar.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    SidebarComponent,
    EmployerStepCompanyComponent,
    EmployerStepWorkspaceComponent,
    EmployerStepSubscriptionComponent,
    EmployerStepReviewComponent,
  ],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss',
})
export class RegistrationComponent {
  private readonly router = inject(Router);
  employerService = inject(EmployerRegistrationService);

  currentStep = this.employerService.currentStep;
  isLoading = this.employerService.loading;

  nextStep() {
    const step = this.currentStep();

    // Block advancing while the current step has invalid fields
    if (!this.employerService.isStepValid(step)) {
      Swal.fire({
        icon: 'warning',
        title: 'Incomplete Step',
        text: 'Please fill in all required fields correctly before continuing.',
        confirmButtonColor: '#e63946',
      });
      return;
    }

    if (step === 4) {
      this.submit();
      return;
    }

    this.employerService.nextStep();
  }

  prevStep() {
    this.employerService.prevStep();
  }

  submit() {
    this.employerService.submit().subscribe({
      next: () => {
        this.employerService.loading.set(false);
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
          queryParams: { email: this.employerService.profile().email },
        });
      },
      error: (err: any) => {
        this.employerService.loading.set(false);
        const message = err?.error?.message || err?.message || 'Registration failed. Please try again.';
        this.employerService.error.set(message);
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