import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import Swal from 'sweetalert2';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Check if user is authenticated
  if (!authService.isAuthenticated()) {
    Swal.fire({
      icon: 'warning',
      title: 'Not Authorized',
      text: 'Please login first to access this page.',
      confirmButtonColor: '#3085d6',
    });
    router.navigateByUrl('/login');
    return false;
  }

  // Check if user has Admin role
  if (!authService.isAdmin()) {
    Swal.fire({
      icon: 'error',
      title: 'Access Denied',
      text: 'You need administrator privileges to access this page.',
      confirmButtonColor: '#3085d6',
    });
    router.navigateByUrl('/');
    return false;
  }

  return true;
};
