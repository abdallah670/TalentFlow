import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-select-workspace',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './select-workspace.component.html',
  styleUrl: './select-workspace.component.scss',
})
export class SelectWorkspaceComponent {
  private readonly router = inject(Router);
  readonly authService = inject(AuthService);

  workspaces = [
    {
      id: '1',
      name: 'Acme Corp Global',
      icon: 'business',
      role: 'Admin',
      jobCount: 1240,
      color: 'primary',
    },
    {
      id: '2',
      name: 'TechFlow Startups',
      icon: 'rocket_launch',
      role: 'Recruiter',
      jobCount: 42,
      color: 'tertiary',
    },
    {
      id: '3',
      name: 'HealthPlus Network',
      icon: 'medication',
      role: 'Viewer',
      jobCount: 856,
      color: 'secondary',
    },
  ];

  userAvatarUrl = 'https://lh3.googleusercontent.com/aida-public/AB6AXuDddlWla1hhRDBBc4EV77O2bBzrOfQXloGhzFN_h-ih7ovH1ThEUOZHpiuGm7mfrQdwM47CN5fIJnfjogS1S7lCjyRYeYSYKJ1N46NT9Bl5oaDKbIrYDYZmbpnnQBhsQH0i-xpxVuZmhhvfVl7WZvPxa3s05oGH3UpkxEAEBXSzh80J5ddre-_0XNfeWZfLOtCvGLI6WVDkPImw8AknvoUfsfw0EAQITrZnDN7X2Mzqrgt7mrSqI8EG1aQQwwhmSlLhGaemy3lGKX0';

  selectWorkspace(workspaceId: string): void {
    this.authService.selectTenant(workspaceId);
    this.router.navigate(['/']);
  }

  logout(): void {
    this.authService.logout();
  }
}
