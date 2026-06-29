import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthResponse } from '../../data/models/auth.model';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const cookieService = inject(CookieService);
  const authService = inject(AuthService);

  const token = cookieService.get('token');

  // Clone the request and add the Bearer token
  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
      withCredentials: true,
    });
  } else {
    // Still include credentials even without token
    authReq = req.clone({
      withCredentials: true,
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // If we get a 401 Unauthorized, try to refresh the token
      if (error.status === 401 && !req.url.includes('/login') && !req.url.includes('/refresh')) {
        return authService.refreshToken().pipe(
          switchMap((response: AuthResponse) => {
            if (response) {
              const newToken = cookieService.get('token');
              const retryReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${newToken}`,
                },
                withCredentials: true,
              });
              return next(retryReq);
            }
            return throwError(() => error);
          }),
          catchError(() => {
            authService.logout();
            return throwError(() => error);
          }),
        );
      }
      return throwError(() => error);
    }),
  );
};
