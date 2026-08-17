import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { adminGuard } from './core/guards/admin-guard';

export const routes: Routes = [
  { path: '', redirectTo: '', pathMatch: 'full' },

  { 
    path: '', 
    loadComponent: () => import('./presentation/pages/home/home.component').then(m => m.HomeComponent) 
  },

  { 
    path: 'login', 
    loadComponent: () => import('./presentation/pages/auth/login/login').then(m => m.Login) 
  },
  { 
    path: 'register', 
    pathMatch: 'full',
    loadComponent: () => import('./presentation/pages/auth/register/register-choice/register-choice.component').then(m => m.RegisterChoiceComponent) 
  },
  { 
    path: 'register/employer', 
    loadComponent: () => import('./presentation/pages/auth/empolyer-registeration/company-setup/company-setup.component').then(m => m.CompanySetupComponent) 
  },
  {
    path: 'register/workspace',
    loadComponent: () => import('./presentation/pages/auth/empolyer-registeration/workspace/workspace.component').then(m => m.WorkspaceComponent)
  },
  {
    path: 'register/subscription',
    loadComponent: () => import('./presentation/pages/auth/empolyer-registeration/subscription/subscription.component').then(m => m.SubscriptionComponent)
  },
  {
    path: 'register/review',
    loadComponent: () => import('./presentation/pages/auth/empolyer-registeration/review/review.component').then(m => m.ReviewComponent)
  },
  { 
    path: 'verify-email', 
    loadComponent: () => import('./presentation/pages/auth/verify-email/verify-email.component').then(m => m.VerifyEmailComponent) 
  },
  { 
    path: 'forgot-password', 
    loadComponent: () => import('./presentation/pages/auth/password/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) 
  },
  { 
    path: 'reset-password', 
    loadComponent: () => import('./presentation/pages/auth/password/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) 
  },
  {
    path: 'setup-account',
    loadComponent: () => import('./presentation/pages/auth/setup-account/setup-account.component').then(m => m.SetupAccountComponent)
  },
  {
    path: 'select-workspace',
    loadComponent: () => import('./presentation/pages/auth/select-workspace/select-workspace.component').then(m => m.SelectWorkspaceComponent)
  },
  
  { path: '**', redirectTo: '' },
];
