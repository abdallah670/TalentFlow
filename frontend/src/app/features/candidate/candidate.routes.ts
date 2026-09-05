import { Routes } from '@angular/router';

/**
 * Candidate portal routes (designs from Design/candidate_*).
 * Pages are standalone and lazily loaded.
 */
export const CANDIDATE_ROUTES: Routes = [
  {
    path: 'account-verified',
    loadComponent: () =>
      import('./pages/account-verified/account-verified.component').then(
        (m) => m.AccountVerifiedComponent,
      ),
  },
  {
    path: 'assessment-results',
    loadComponent: () =>
      import('./pages/assessment-results/assessment-results.component').then(
        (m) => m.AssessmentResultsComponent,
      ),
  },
  {
    path: 'comparison-matrix',
    loadComponent: () =>
      import('./pages/comparison-matrix/comparison-matrix.component').then(
        (m) => m.ComparisonMatrixComponent,
      ),
  },
  {
    path: 'help-center',
    loadComponent: () =>
      import('./pages/help-center/help-center.component').then(
        (m) => m.HelpCenterComponent,
      ),
  },
  {
    path: 'interview-prep-guide',
    loadComponent: () =>
      import('./pages/interview-prep-guide/interview-prep-guide.component').then(
        (m) => m.InterviewPrepGuideComponent,
      ),
  },
  {
    path: 'interview-preparation',
    loadComponent: () =>
      import('./pages/interview-preparation/interview-preparation.component').then(
        (m) => m.InterviewPreparationComponent,
      ),
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./pages/profile/profile.component').then(
        (m) => m.ProfileComponent,
      ),
  },
  {
    path: 'technical-assessment',
    loadComponent: () =>
      import('./pages/technical-assessment/technical-assessment.component').then(
        (m) => m.TechnicalAssessmentComponent,
      ),
  },
];
