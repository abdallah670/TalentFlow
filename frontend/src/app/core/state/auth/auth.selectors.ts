import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState } from './auth.reducer';

export const selectAuthState = createFeatureSelector<AuthState>('auth');

export const selectUser = createSelector(selectAuthState, (state: AuthState) => state.user);

export const selectToken = createSelector(selectAuthState, (state: AuthState) => state.token);

export const selectIsAuthenticated = createSelector(
  selectAuthState,
  (state: AuthState) => state.isAuthenticated,
);

export const selectAuthLoading = createSelector(
  selectAuthState,
  (state: AuthState) => state.loading,
);

export const selectAuthError = createSelector(
  selectAuthState,
  (state: AuthState) => state.error,
);

export const selectAvailableTenants = createSelector(
  selectAuthState,
  (state: AuthState) => state.availableTenants,
);

export const selectSelectedTenantId = createSelector(
  selectAuthState,
  (state: AuthState) => state.selectedTenantId,
);

export const selectIsInvited = createSelector(
  selectAuthState,
  (state: AuthState) => state.isInvited,
);

export const selectInvitationToken = createSelector(
  selectAuthState,
  (state: AuthState) => state.invitationToken,
);

export const selectRegistrationStep = createSelector(
  selectAuthState,
  (state: AuthState) => state.registrationStep,
);

export const selectRegistrationData = createSelector(
  selectAuthState,
  (state: AuthState) => state.registrationData,
);
