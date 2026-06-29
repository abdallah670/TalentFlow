import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { Observable, tap, catchError, of } from 'rxjs';
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

@Injectable({
  providedIn: 'root',
})
export class AuthService{
  private readonly http = inject(HttpClient);
  private readonly cookieService = inject(CookieService);

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
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response: AuthResponse) => this.handleAuthentication(response))
    );
  }

  register(request: RegistrationRequest): Observable<RegistrationResponse> {
    return this.http.post<RegistrationResponse>(`${this.apiUrl}/register`, request).pipe(
      tap((response: RegistrationResponse) => {
        // Only authenticate if email verification is not required
        if (!response.requiresEmailVerification && response.token) {
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

  // Email verification methods
  verifyEmail(request: VerifyEmailRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/verify-email`, request);
  }

  resendVerificationEmail(request: ResendVerificationRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/resend-verification`, request);
  }

  // Password reset methods
  forgotPassword(request: ForgotPasswordRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/forgot-password`, request);
  }

  resetPassword(request: ResetPasswordRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/reset-password`, request);
  }

  logout(): void {
    this.cookieService.delete('token', '/');
    this.cookieService.delete('refreshToken', '/');
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.userRoles.set([]);
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

  private handleAuthentication(response: AuthResponse): void {
    // Store in secure cookies
    this.cookieService.set('token', response.token, 7, '/');
    if (response.refreshToken) {
      this.cookieService.set('refreshToken', response.refreshToken, 7, '/');
    }

    console.log('[AuthService] Token stored in cookies:', response.token.substring(0, 20) + '...');
    console.log('[AuthService] Refresh token stored:', response.refreshToken ? 'Yes' : 'No');

    // Parse roles from JWT token
    const roles = this.parseRolesFromToken(response.token);
    this.userRoles.set(roles);
    console.log('[AuthService] User roles:', roles);

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
    console.log('[AuthService] isAuthenticated set to:', this.isAuthenticated());
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
      // ASP.NET Identity uses this claim type for roles
      const roleClaim = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload['role'];
      if (Array.isArray(roleClaim)) {
        return roleClaim;
      }
      return roleClaim ? [roleClaim] : [];
    } catch {
      return [];
    }
  }

  // Handle OAuth social login callback
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

        // Map common JWT claims to our user object
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
