import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./features/home/home.routes').then((m) => m.HOME_ROUTES),
  },
  {
    path: '',
    loadChildren: () =>
      import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
  },
  {
    path: 'candidate',
    loadChildren: () =>
      import('./features/candidate/candidate.routes').then(
        (m) => m.CANDIDATE_ROUTES,
      ),
  },
  { path: '**', redirectTo: '' },
];
