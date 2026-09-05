import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login/login').then((m) => m.Login),
  },
  {
    path: 'register',
    pathMatch: 'full',
    loadComponent: () =>
      import('./pages/register-choice/register-choice.component').then(
        (m) => m.RegisterChoiceComponent,
      ),
  },
  {
    path: 'register/employer',
    loadComponent: () =>
      import('./pages/employer-registration/registration.component').then(
        (m) => m.RegistrationComponent,
      ),
  },
  {
    path: 'register/candidate',
    loadComponent: () =>
      import('./pages/candidate-registration/registration.component').then(
        (m) => m.RegistrationComponent,
      ),
  },
  {
    path: 'verify-email',
    loadComponent: () =>
      import('./pages/verify-email/verify-email.component').then(
        (m) => m.VerifyEmailComponent,
      ),
  },
  {
    path: 'forgot-password',
    loadComponent: () =>
      import('./pages/forgot-password/forgot-password.component').then(
        (m) => m.ForgotPasswordComponent,
      ),
  },
  {
    path: 'reset-password',
    loadComponent: () =>
      import('./pages/reset-password/reset-password.component').then(
        (m) => m.ResetPasswordComponent,
      ),
  },
  {
    path: 'setup-account',
    loadComponent: () =>
      import('./pages/setup-account/setup-account.component').then(
        (m) => m.SetupAccountComponent,
      ),
  },
  {
    path: 'select-workspace',
    loadComponent: () =>
      import('./pages/select-workspace/select-workspace.component').then(
        (m) => m.SelectWorkspaceComponent,
      ),
  },
];
