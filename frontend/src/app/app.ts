import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { NavbarComponent } from '@shared/components/navbar/navbar.component';
import { BackButtonComponent } from '@shared/ui/back-button/back-button.component';
import { AuthService } from '@features/auth/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, BackButtonComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = 'TalentFlow';

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Track the current URL reactively
  private readonly currentUrl = signal(this.router.url);
  constructor() {
    this.router.events.subscribe((e) => {
      if (e instanceof NavigationEnd) {
        this.currentUrl.set(e.urlAfterRedirects);
      }
    });
  }

  // Expose auth state from AuthService
  protected get isAuthenticated() {
    return this.authService.isAuthenticated;
  }

  // Check if current route is an auth page (login/register)
  protected get isAuthPage(): boolean {
    const url = this.currentUrl();
    return url.includes('/login') || url.includes('/register') ||
           url.includes('/verify-email') || url.includes('/forgot-password') ||
           url.includes('/reset-password');
  }

  // Show navbar only when authenticated AND not on auth pages
  protected get showNavbar(): boolean {
    return this.isAuthenticated() && !this.isAuthPage;
  }

  // Show the floating back button on every page except home
  protected get showBackButton(): boolean {
    const url = this.currentUrl().split('?')[0];
    return url !== '/' && url !== '';
  }
}