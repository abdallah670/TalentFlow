import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { Store } from '@ngrx/store';
import Swal from 'sweetalert2';
import { selectAuthState } from '@features/auth/state/auth.selectors';

export const authGuard: CanActivateFn = (route, state) => {
  const store = inject(Store);
  const router = inject(Router);

  const authState = store.selectSignal(selectAuthState)();

  if (!authState.isAuthenticated) {
    Swal.fire({
      icon: 'warning',
      title: 'Not Authorized',
      text: 'Please login first to access this page.',
      confirmButtonColor: '#3085d6',
    });
    router.navigateByUrl('/login');
    return false;
  }

  if (!authState.user?.emailConfirmed) {
    Swal.fire({
      icon: 'info',
      title: 'Email Verification Required',
      text: 'Please verify your email address to continue using the application.',
      confirmButtonColor: '#3085d6',
    });
    const email = authState.user?.email || '';
    router.navigate(['/verify-email'], { queryParams: { email } });
    return false;
  }

  // Multi-tenant check: if user belongs to multiple tenants and hasn't selected one, redirect to workspace picker
  if (authState.availableTenants.length > 1 && !authState.selectedTenantId) {
    router.navigate(['/select-workspace']);
    return false;
  }

  // Invited user check: if user is invited but hasn't set up account, redirect to setup
  if (authState.isInvited) {
    router.navigate(['/setup-account']);
    return false;
  }

  return true;
};
