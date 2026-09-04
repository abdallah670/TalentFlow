import { Component, inject } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { NavbarComponent } from '@shared/components/navbar/navbar.component';
import { AuthService } from '@features/auth/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = 'TalentFlow';
  
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  
  // Expose auth state from AuthService
  protected get isAuthenticated() {
    return this.authService.isAuthenticated;
  }
  
  // Check if current route is an auth page (login/register)
  protected get isAuthPage(): boolean {
    const url = this.router.url;
    return url.includes('/login') || url.includes('/register') || 
           url.includes('/verify-email') || url.includes('/forgot-password') || 
           url.includes('/reset-password');
  }
  
  // Show navbar only when authenticated AND not on auth pages
  protected get showNavbar(): boolean {
    return this.isAuthenticated() && !this.isAuthPage;
  }
}