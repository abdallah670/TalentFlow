import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { exhaustMap, map, catchError, tap, of } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { AuthActions } from './auth.actions';
import { AuthResponse, RegistrationResponse } from '@features/auth/models/auth.model';
import { TenantInfo } from '@features/auth/models/registration.model';

export const login$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.login),
      exhaustMap(({ request }) =>
        authService.login(request).pipe(
          map((response: AuthResponse) => AuthActions.loginSuccess({ response })),
          catchError((error) =>
            of(AuthActions.loginFailure({ error: error?.message || 'Login failed' })),
          ),
        ),
      ),
    );
  },
  { functional: true },
);

export const register$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.register),
      exhaustMap(({ request }) =>
        authService.register(request).pipe(
          map((response: RegistrationResponse) => AuthActions.registerSuccess({ response })),
          catchError((error) =>
            of(AuthActions.registerFailure({ error: error?.message || 'Registration failed' })),
          ),
        ),
      ),
    );
  },
  { functional: true },
);

export const registerCandidate$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.registerCandidate),
      exhaustMap(({ request }) =>
        authService.registerCandidate(request).pipe(
          map((response: RegistrationResponse) => AuthActions.registerCandidateSuccess({ response })),
          catchError((error) =>
            of(AuthActions.registerCandidateFailure({ error: error?.message || 'Candidate registration failed' })),
          ),
        ),
      ),
    );
  },
  { functional: true },
);

export const registerEmployer$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.registerEmployer),
      exhaustMap(({ request }) =>
        authService.registerEmployer(request).pipe(
          map((response: RegistrationResponse) => AuthActions.registerEmployerSuccess({ response })),
          catchError((error) =>
            of(AuthActions.registerEmployerFailure({ error: error?.message || 'Employer registration failed' })),
          ),
        ),
      ),
    );
  },
  { functional: true },
);

export const acceptInvitation$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.acceptInvitation),
      exhaustMap(({ request }) =>
        authService.acceptInvitation(request).pipe(
          map((response: AuthResponse) => AuthActions.acceptInvitationSuccess({ response })),
          catchError((error) =>
            of(AuthActions.acceptInvitationFailure({ error: error?.message || 'Accept invitation failed' })),
          ),
        ),
      ),
    );
  },
  { functional: true },
);

export const loadTenants$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.loginSuccess),
      exhaustMap(() =>
        authService.getAvailableTenants().pipe(
          catchError(() => of([] as TenantInfo[])),
        ),
      ),
    );
  },
  { functional: true, dispatch: false },
);

export const logout$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService), router = inject(Router)) => {
    return actions$.pipe(
      ofType(AuthActions.logout),
      tap(() => {
        authService.logout();
        router.navigate(['/login']);
      }),
    );
  },
  { functional: true, dispatch: false },
);

export const refreshToken$ = createEffect(
  (actions$ = inject(Actions), authService = inject(AuthService)) => {
    return actions$.pipe(
      ofType(AuthActions.refreshToken),
      exhaustMap(() =>
        authService.refreshToken().pipe(
          map((response: AuthResponse) => AuthActions.refreshTokenSuccess({ response })),
          catchError((error) =>
            of(
              AuthActions.refreshTokenFailure({ error: error?.message || 'Token refresh failed' }),
            ),
          ),
        ),
      ),
    );
  },
  { functional: true },
);

export const loginSuccess$ = createEffect(
  (actions$ = inject(Actions), router = inject(Router)) => {
    return actions$.pipe(
      ofType(AuthActions.loginSuccess, AuthActions.acceptInvitationSuccess),
      tap(() => router.navigate(['/'])),
    );
  },
  { functional: true, dispatch: false },
);
