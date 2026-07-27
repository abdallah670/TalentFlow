import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  private readonly authService = inject(AuthService);

  sidebarOpen = signal(false);

  get currentUser() {
    return this.authService.currentUser;
  }

  get isAdmin() {
    return this.authService.isAdmin;
  }

  toggleSidebar(): void {
    this.sidebarOpen.update(value => !value);
  }

  logout(): void {
    this.authService.logout();
  }

  getUserInitials(): string {
    const user = this.currentUser();
    if (!user) return '?';
    const name = user.userName || user.email || '';
    return name.charAt(0).toUpperCase();
  }
}
