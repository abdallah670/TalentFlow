import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateRegistrationService } from '../../../../core/services/registration.service';


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
}
