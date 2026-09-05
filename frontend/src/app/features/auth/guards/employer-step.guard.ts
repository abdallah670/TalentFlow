import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { EmployerRegistrationService } from '../services/employer-registration.service';

/**
 * Blocks access to a later employer-onboarding step until every
 * previous step has been completed with a valid form.
 * Usage: canActivate: [employerStepGuard], data: { stepKey: 'workspace' }
 */
export const employerStepGuard: CanActivateFn = (route) => {
  const employerService = inject(EmployerRegistrationService);
  const router = inject(Router);
  const stepKey = route.data['stepKey'] as string;

  if (employerService.isStepAccessible(stepKey)) {
    return true;
  }

  Swal.fire({
    icon: 'warning',
    title: 'Complete the previous steps first',
    text: 'Please fill in all required fields of the previous steps before continuing.',
    confirmButtonColor: '#24389c',
  });
  return router.createUrlTree(['/register/employer']);
};
