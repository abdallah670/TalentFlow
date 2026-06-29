import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import Swal from 'sweetalert2';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Access the signal value correctly by calling it
  const isAuth = authService.isAuthenticated();

  if (!isAuth) {
    Swal.fire({
      icon: 'warning',
      title: 'Not Authorized',
      text: 'Please login first to access this page.',
      confirmButtonColor: '#3085d6',
    });
    router.navigateByUrl('/login');
    return false;
  }

  // Check if email is confirmed
  const isEmailConfirmed = authService.isEmailConfirmed();
  if (!isEmailConfirmed) {
    Swal.fire({
      icon: 'info',
      title: 'Email Verification Required',
      text: 'Please verify your email address to continue using the application.',
      confirmButtonColor: '#3085d6',
    });
    const email = authService.currentUser()?.email || '';
    router.navigate(['/verify-email'], { queryParams: { email: email } });
    return false;
  }

  // Check if user has at least Planner role for regular content access

  const isAdmin = authService.isAdmin();
  
  if (!isAdmin) {
    Swal.fire({
      icon: 'warning',
      title: 'Access Denied',
      text: 'You need a Planner or Admin role to access this page.',
      confirmButtonColor: '#3085d6',
    });
    router.navigateByUrl('/login');
    return false;
  }

  return true;
};
