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
    loadComponent: () => import('./presentation/pages/auth/register/register').then(m => m.Register) 
  },
  {
    path: 'register/company-details',
    loadComponent: () => import('./presentation/pages/auth/register/company-details/company-details.component').then(m => m.CompanyDetailsComponent)
  },
  {
    path: 'register/select-plan',
    loadComponent: () => import('./presentation/pages/auth/register/select-plan/select-plan.component').then(m => m.SelectPlanComponent)
  },
  {
    path: 'register/candidate',
    loadComponent: () => import('./presentation/pages/auth/register/candidate-registration/candidate-registration.component').then(m => m.CandidateRegistrationComponent),
    children: [
      { path: 'step1', loadComponent: () => import('./presentation/pages/auth/register/candidate-registration/step1/step1.component').then(m => m.Step1Component) },
      { path: 'step2', loadComponent: () => import('./presentation/pages/auth/register/candidate-registration/step2/step2.component').then(m => m.Step2Component) },
      { path: 'step3', loadComponent: () => import('./presentation/pages/auth/register/candidate-registration/step3/step3.component').then(m => m.Step3Component) },
      { path: 'step4', loadComponent: () => import('./presentation/pages/auth/register/candidate-registration/step4/step4.component').then(m => m.Step4Component) },
      { path: '', redirectTo: 'step1', pathMatch: 'full' },
    ]
  },
  { 
    path: 'verify-email', 
    loadComponent: () => import('./presentation/pages/auth/verify-email/verify-email.component').then(m => m.VerifyEmailComponent) 
  },
  { 
    path: 'forgot-password', 
    loadComponent: () => import('./presentation/pages/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) 
  },
  { 
    path: 'reset-password', 
    loadComponent: () => import('./presentation/pages/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) 
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
