import { createActionGroup, emptyProps, props } from '@ngrx/store';
import {
  AuthRequest,
  AuthResponse,
  RegistrationRequest,
  RegistrationResponse,
  CandidateRegistrationRequest,
  EmployerRegistrationRequest,
  AcceptInvitationRequest,
  TenantInfo,
} from '../../../data/models/auth.model';

export const AuthActions = createActionGroup({
  source: 'Auth',
  events: {
    Login: props<{ request: AuthRequest }>(),
    'Login Success': props<{ response: AuthResponse }>(),
    'Login Failure': props<{ error: string }>(),

    Register: props<{ request: RegistrationRequest }>(),
    'Register Success': props<{ response: RegistrationResponse }>(),
    'Register Failure': props<{ error: string }>(),

    'Register Candidate': props<{ request: CandidateRegistrationRequest }>(),
    'Register Candidate Success': props<{ response: RegistrationResponse }>(),
    'Register Candidate Failure': props<{ error: string }>(),

    'Register Employer': props<{ request: EmployerRegistrationRequest }>(),
    'Register Employer Success': props<{ response: RegistrationResponse }>(),
    'Register Employer Failure': props<{ error: string }>(),

    Logout: emptyProps(),

    'Refresh Token': emptyProps(),
    'Refresh Token Success': props<{ response: AuthResponse }>(),
    'Refresh Token Failure': props<{ error: string }>(),

    'Check Auth': emptyProps(),

    'Set Available Tenants': props<{ tenants: TenantInfo[] }>(),
    'Select Tenant': props<{ tenantId: string }>(),

    'Set Invitation': props<{ token: string; email: string }>(),
    'Accept Invitation': props<{ request: AcceptInvitationRequest }>(),
    'Accept Invitation Success': props<{ response: AuthResponse }>(),
    'Accept Invitation Failure': props<{ error: string }>(),

    'Reset Registration': emptyProps(),
  },
});
