import { createReducer, on } from '@ngrx/store';
import { AuthActions } from './auth.actions';
import { TenantInfo } from '../../../data/models/registration.model';


export interface AuthState {
  user: { id: string; email: string; username: string; roles: string[]; emailConfirmed: boolean } | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
  registrationStep: number;
  registrationData: any;
  availableTenants: TenantInfo[];
  selectedTenantId: string | null;
  isInvited: boolean;
  invitationToken: string | null;
}

export const initialState: AuthState = {
  user: null,
  token: null,
  isAuthenticated: false,
  loading: false,
  error: null,
  registrationStep: 0,
  registrationData: {},
  availableTenants: [],
  selectedTenantId: null,
  isInvited: false,
  invitationToken: null,
};

export const authReducer = createReducer(
  initialState,

  // Login
  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(AuthActions.loginSuccess, (state, { response }) => ({
    ...state,
    user: {
      id: response.id,
      email: response.email,
      username: response.userName,
      roles: [],
      emailConfirmed: false,
    },
    token: response.token ?? null,
    isAuthenticated: true,
    loading: false,
    error: null,
  })),
  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  // Register
  on(AuthActions.register, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(AuthActions.registerSuccess, (state, { response }) => ({
    ...state,
    user: {
      id: response.userId,
      email: response.email,
      username: response.userName,
      roles: [],
      emailConfirmed: false,
    },
    token: response.token ?? null,
    isAuthenticated: true,
    loading: false,
    error: null,
  })),
  on(AuthActions.registerFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  // Register Candidate
  on(AuthActions.registerCandidate, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(AuthActions.registerCandidateSuccess, (state, { response }) => ({
    ...state,
    user: {
      id: response.userId,
      email: response.email,
      username: response.userName,
      roles: [],
      emailConfirmed: false,
    },
    token: response.token ?? null,
    isAuthenticated: true,
    loading: false,
    error: null,
  })),
  on(AuthActions.registerCandidateFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  // Register Employer
  on(AuthActions.registerEmployer, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(AuthActions.registerEmployerSuccess, (state, { response }) => ({
    ...state,
    user: {
      id: response.userId,
      email: response.email,
      username: response.userName,
      roles: [],
      emailConfirmed: false,
    },
    token: response.token ?? null,
    isAuthenticated: true,
    loading: false,
    error: null,
  })),
  on(AuthActions.registerEmployerFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  // Logout
  on(AuthActions.logout, () => initialState),

  // Token Refresh
  on(AuthActions.refreshToken, (state) => ({
    ...state,
    loading: true,
  })),
  on(AuthActions.refreshTokenSuccess, (state, { response }) => ({
    ...state,
    user: {
      id: response.id,
      email: response.email,
      username: response.userName,
      roles: [],
      emailConfirmed: false,
    },
    token: response.token ?? null,
    isAuthenticated: true,
    loading: false,
  })),
  on(AuthActions.refreshTokenFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  // Tenants
  on(AuthActions.setAvailableTenants, (state, { tenants }) => ({
    ...state,
    availableTenants: tenants,
  })),
  on(AuthActions.selectTenant, (state, { tenantId }) => ({
    ...state,
    selectedTenantId: tenantId,
  })),

  // Invitation
  on(AuthActions.setInvitation, (state, { token, email }) => ({
    ...state,
    invitationToken: token,
    isInvited: true,
    user: {
      ...state.user,
      email,
    } as any,
  })),
  on(AuthActions.acceptInvitationSuccess, (state, { response }) => ({
    ...state,
    user: {
      id: response.id,
      email: response.email,
      username: response.userName,
      roles: [],
      emailConfirmed: true,
    },
    token: response.token ?? null,
    isAuthenticated: true,
    isInvited: false,
    invitationToken: null,
    loading: false,
    error: null,
  })),
  on(AuthActions.acceptInvitationFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  // Registration state
  on(AuthActions.resetRegistration, () => ({
    ...initialState,
  })),
);
