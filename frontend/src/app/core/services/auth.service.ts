import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { Observable, tap, catchError, of, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AuthRequest,
  AuthResponse,
  RegistrationRequest,
  RegistrationResponse,

  VerifyEmailRequest,
  ResendVerificationRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
 
} from '../../data/models/auth.model';
import { Store } from '@ngrx/store';
import { AuthActions } from '../state/auth/auth.actions';
import { AcceptInvitationRequest, CandidateRegistrationRequest, EmployerRegistrationRequest, TenantInfo } from '../../data/models/registration.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly cookieService = inject(CookieService);
  private readonly store = inject(Store);

  private readonly apiUrl = `${environment.baseUrl}/Auth`;

  // Signals for reactive state
  currentUser = signal<any>(null);
  isAuthenticated = signal<boolean>(false);
  userRoles = signal<string[]>([]);

  // Computed role checks
  isAdmin = computed(() => this.userRoles().includes('Admin'));
  hasRole = (role: string) => computed(() => this.userRoles().includes(role));
  isEmailConfirmed = computed(() => this.currentUser()?.emailConfirmed ?? false);

  constructor() {
    this.checkAuthStatus();
  }

  login(request: AuthRequest): Observable<AuthResponse> {
    this.store.dispatch(AuthActions.login({ request }));
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response: AuthResponse) => this.handleAuthentication(response))
    );
  }

  register(request: RegistrationRequest): Observable<RegistrationResponse> {
    this.store.dispatch(AuthActions.register({ request }));
    return this.http.post<any>(`${this.apiUrl}/register`, request).pipe(
      map((response: any) => this.mapAuthResponse(response)),
      tap((response: RegistrationResponse) => {
        if (response.token) {
          this.handleAuthentication({
            id: response.userId,
            userName: response.userName,
            email: response.email,
            token: response.token,
            refreshToken: response.refreshToken || '',
          });
        }
      }),
    );
  }

  registerCandidate(request: CandidateRegistrationRequest): Observable<RegistrationResponse> {
    this.store.dispatch(AuthActions.registerCandidate({ request }));
    return this.http.post<any>(`${this.apiUrl}/register-candidate`, request).pipe(
      map((response: any) => this.mapAuthResponse(response)),
      tap((response: RegistrationResponse) => {
        if (response.token) {
          this.handleAuthentication({
            id: response.userId,
            userName: response.userName,
            email: response.email,
            token: response.token,
            refreshToken: response.refreshToken || '',
          });
        }
      }),
    );
  }

  registerEmployer(request: EmployerRegistrationRequest): Observable<RegistrationResponse> {
    this.store.dispatch(AuthActions.registerEmployer({ request }));
    return this.http.post<any>(`${this.apiUrl}/register-employer`, request).pipe(
      map((response: any) => this.mapAuthResponse(response)),
      tap((response: RegistrationResponse) => {
        if (response.token) {
          this.handleAuthentication({
            id: response.userId,
            userName: response.userName,
            email: response.email,
            token: response.token,
            refreshToken: response.refreshToken || '',
          });
        }
      }),
    );
  }

  getAvailableTenants(): Observable<TenantInfo[]> {
    return this.http.get<TenantInfo[]>(`${this.apiUrl}/tenants`).pipe(
      tap((tenants) => {
        this.store.dispatch(AuthActions.setAvailableTenants({ tenants }));
      }),
    );
  }

  selectTenant(tenantId: string): void {
    this.store.dispatch(AuthActions.selectTenant({ tenantId }));
  }

  acceptInvitation(request: AcceptInvitationRequest): Observable<AuthResponse> {
    this.store.dispatch(AuthActions.acceptInvitation({ request }));
    return this.http.post<AuthResponse>(`${this.apiUrl}/accept-invitation`, request).pipe(
      tap((response: AuthResponse) => this.handleAuthentication(response))
    );
  }

  verifyEmail(request: VerifyEmailRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/verify-email`, request);
  }

  resendVerificationEmail(request: ResendVerificationRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/resend-verification`, request);
  }

  forgotPassword(request: ForgotPasswordRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/forgot-password`, request);
  }

  resetPassword(request: ResetPasswordRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/reset-password`, request);
  }

  checkEmailStatus(email: string): Observable<{ isRegistered: boolean }> {
    return this.http.post<{ isRegistered: boolean }>(`${this.apiUrl}/email-status`, { email });
  }

  logout(): void {
    this.cookieService.delete('token', '/');
    this.cookieService.delete('refreshToken', '/');
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.userRoles.set([]);
    this.store.dispatch(AuthActions.logout());
  }

  refreshToken(): Observable<AuthResponse> {
    const token = this.cookieService.get('token');
    const refreshToken = this.cookieService.get('refreshToken');

    if (!token || !refreshToken) {
      this.logout();
      return of(null as any);
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh`, { Token: token, RefreshToken: refreshToken }).pipe(
      tap((response: AuthResponse) => this.handleAuthentication(response)),
      catchError(() => {
        this.logout();
        return of(null as any);
      }),
    );
  }

  private mapAuthResponse(response: any): RegistrationResponse {
    const isAuthenticated = response?.isAuthenticated ?? response?.IsAuthenticated ?? false;
    const message = response?.message ?? response?.Message ?? '';

    if (!isAuthenticated) {
      throw { error: { message: message || 'Registration failed. Please try again.' } };
    }

    return {
      userId: response?.id ?? response?.Id ?? '',
      token: response?.token ?? response?.Token ?? undefined,
      refreshToken: response?.refreshToken ?? response?.RefreshToken ?? undefined,
      email: response?.email ?? response?.Email ?? '',
      userName: response?.userName ?? response?.UserName ?? '',
      requiresEmailVerification: true,
    };
  }

  private handleAuthentication(response: AuthResponse): void {
    this.cookieService.set('token', response.token, 7, '/');
    if (response.refreshToken) {
      this.cookieService.set('refreshToken', response.refreshToken, 7, '/');
    }

    const roles = this.parseRolesFromToken(response.token);
    this.userRoles.set(roles);

    const payload = this.decodeToken(response.token);
    const emailConfirmed = payload ? (payload['email_confirmed'] === 'true' || payload['email_confirmed'] === true) : false;

    this.currentUser.set({
      id: response.id,
      userName: response.userName,
      email: response.email,
      roles: roles,
      emailConfirmed: emailConfirmed
    });
    this.isAuthenticated.set(true);
  }

  private decodeToken(token: string): any {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload;
    } catch {
      return null;
    }
  }

  private parseRolesFromToken(token: string): string[] {
    const payload = this.decodeToken(token);
    if (!payload) return [];
    
    try {
      const roleClaim = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload['role'];
      if (Array.isArray(roleClaim)) {
        return roleClaim;
      }
      return roleClaim ? [roleClaim] : [];
    } catch {
      return [];
    }
  }

  handleSocialLogin(response: AuthResponse): void {
    this.handleAuthentication(response);
  }

  private checkAuthStatus(): void {
    const token = this.cookieService.get('token');
    if (token) {
      const payload = this.decodeToken(token);
      if (payload) {
        this.isAuthenticated.set(true);
        const roles = this.parseRolesFromToken(token);
        this.userRoles.set(roles);

        const userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload['sub'];
        const userName = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || payload['unique_name'];
        const email = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || payload['email'];
        const emailConfirmed = payload['email_confirmed'] === 'true' || payload['email_confirmed'] === true;

        this.currentUser.set({
          id: userId,
          userName: userName,
          email: email,
          roles: roles,
          emailConfirmed: emailConfirmed
        });
        
        console.log('[AuthService] Restored session for:', userName);
      } else {
        this.logout();
      }
    }
  }
}
