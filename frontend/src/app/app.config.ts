import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  isDevMode,
  ErrorHandler,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors, withFetch } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { CookieService } from 'ngx-cookie-service';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideRouterStore, routerReducer } from '@ngrx/router-store';
import { provideStoreDevtools } from '@ngrx/store-devtools';

import { routes } from './app.routes';
import { tokenInterceptor } from './core/interceptors/token-interceptor';
import { authReducer } from './core/state/auth/auth.reducer';
import * as authEffects from './core/state/auth/auth.effects';
import { GlobalErrorHandler } from './core/errors/global-error-handler';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    provideRouter(routes),
    provideHttpClient(withInterceptors([tokenInterceptor]), withFetch()),
    provideAnimationsAsync(),
    CookieService,
    provideStore({
      auth: authReducer,
      router: routerReducer,
    }),
    provideEffects(
      authEffects,
    ),
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !isDevMode(),
      autoPause: true,
      trace: false,
      traceLimit: 75,
    }),
    provideRouterStore(),
  ],
};