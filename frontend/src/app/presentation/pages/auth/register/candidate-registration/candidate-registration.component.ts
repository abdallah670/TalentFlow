import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../../core/services/auth.service';

@Component({
  selector: 'app-candidate-registration',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './candidate-registration.component.html',
  styleUrl: './candidate-registration.component.scss',
})
export class CandidateRegistrationComponent {
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly authService = inject(AuthService);

  steps = [
    { label: 'Basic Info', route: 'step1', completed: true },
    { label: 'Professional', route: 'step2', completed: false },
    { label: 'Resume & Skills', route: 'step3', completed: false },
    { label: 'Preferences', route: 'step4', completed: false },
  ];

  get currentStepIndex(): number {
    const url = this.router.url;
    const stepMatch = url.match(/step(\d)/);
    return stepMatch ? parseInt(stepMatch[1]) - 1 : 0;
  }

  navigateToStep(stepIndex: number): void {
    this.router.navigate(['step' + (stepIndex + 1)], { relativeTo: this.route });
  }
}
