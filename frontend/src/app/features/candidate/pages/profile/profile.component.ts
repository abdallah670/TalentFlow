import { Component } from '@angular/core';
import { PortalSidebarComponent } from '@shared/components/portal-sidebar/portal-sidebar.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [PortalSidebarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {}
