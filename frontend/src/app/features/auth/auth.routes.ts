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
      import('./pages/employer-registration/company-setup/company-setup.component').then(
        (m) => m.CompanySetupComponent,
      ),
  },
  {
    path: 'register/workspace',
    loadComponent: () =>
      import('./pages/employer-registration/workspace/workspace.component').then(
        (m) => m.WorkspaceComponent,
      ),
  },
  {
    path: 'register/subscription',
    loadComponent: () =>
      import('./pages/employer-registration/subscription/subscription.component').then(
        (m) => m.SubscriptionComponent,
      ),
  },
  {
    path: 'register/review',
    loadComponent: () =>
      import('./pages/employer-registration/review/review.component').then(
        (m) => m.ReviewComponent,
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
