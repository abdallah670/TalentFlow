import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { EmployerRegistrationService } from '@features/auth/services/employer-registration.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './empolyersidebar.component.html',
  styleUrl: './empolyersidebar.component.scss',
})
export class SidebarComponent {
  public router = inject(Router);
  private readonly employerService = inject(EmployerRegistrationService);

  private readonly routes = [
    '/register/employer',
    '/register/workspace',
    '/register/subscription',
    '/register/review',
  ];

  private readonly stepKeys = EmployerRegistrationService.ROUTE_ORDER;

  isCompleted(path: string): boolean {
    const currentIndex = this.routes.indexOf(this.router.url.split('?')[0]);
    const itemIndex = this.routes.indexOf(path);
    return itemIndex < currentIndex;
  }

  /** A nav item is locked when a previous onboarding step is not done yet. */
  isLocked(path: string): boolean {
    const itemIndex = this.routes.indexOf(path);
    const stepKey = this.stepKeys[itemIndex];
    return stepKey !== undefined && !this.employerService.isStepAccessible(stepKey);
  }
}

