import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StepPersonalComponent } from '@features/auth/components/step-personal/step-personal.component';
import { StepProfessionalComponent } from '@features/auth/components/step-professional/step-professional.component';
import { StepSkillsComponent } from '@features/auth/components/step-skills/step-skills.component';
import { StepReviewComponent } from '@features/auth/components/step-review/step-review.component';
import { SidebarComponent } from '@shared/components/candidate-sidebar/candidatesidebar.component';
import { CandidateRegistrationService } from '@features/auth/services/registration.service';
import { RegistrationResponse } from '@features/auth/models/auth.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-registration',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    RouterLink,
    SidebarComponent,
    StepPersonalComponent,
    StepProfessionalComponent,
    StepSkillsComponent,
    StepReviewComponent
  ],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss'
})
export class RegistrationComponent {
  private readonly router = inject(Router);
  registrationService = inject(CandidateRegistrationService);

  currentStep = this.registrationService.currentStep;
  isLoading = this.registrationService.loading;
  error = this.registrationService.error;

  get currentStepTitle(): string {
    return this.registrationService.steps.find(s => s.id === this.currentStep())?.title || '';
  }

  nextStep() {
    const step = this.currentStep();

    // Validate step 1 before allowing advance
    if (step === 1 && !this.registrationService.isStepValid(1)) {
      Swal.fire({
        icon: 'warning',
        title: 'Incomplete Step',
        text: 'Please fill in all required fields correctly before continuing.',
        confirmButtonColor: '#e63946',
      });
      return;
    }

    // On final step, submit instead of advancing
    if (step === 4) {
      this.submitProfile();
      return;
    }

    this.registrationService.nextStep();
  }

  prevStep() {
    this.registrationService.prevStep();
  }

  submitProfile() {
    this.registrationService.submit().subscribe({
      next: (response: RegistrationResponse) => {
        this.registrationService.loading.set(false);
        Swal.fire({
          icon: 'success',
          title: 'Registration Submitted!',
          text: 'Your account has been created. Please check your email to verify your account.',
          timer: 3000,
          showConfirmButton: false,
          position: 'top-end',
          toast: true,
        });
        this.router.navigate(['/verify-email'], {
          queryParams: { email: response.email }
        });
      },
      error: (err: any) => {
        this.registrationService.loading.set(false);
        const message = err?.error?.message || err?.message || 'Registration failed. Please try again.';
        this.registrationService.error.set(message);
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
