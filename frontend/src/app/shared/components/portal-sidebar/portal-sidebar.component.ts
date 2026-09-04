import { Component, Input } from '@angular/core';

export interface PortalNavItem {
  label: string;
  icon: string;
  active?: boolean;
}

@Component({
  selector: 'app-portal-sidebar',
  standalone: true,
  templateUrl: './portal-sidebar.component.html',
  styleUrl: './portal-sidebar.component.scss',
})
export class PortalSidebarComponent {
  @Input() subtitle = 'HR Intelligence';
  @Input() items: PortalNavItem[] = [
    { label: 'Dashboard', icon: 'dashboard' },
    { label: 'Jobs', icon: 'work' },
    { label: 'Pipeline', icon: 'account_tree' },
    { label: 'Candidates', icon: 'group' },
    { label: 'Interviews', icon: 'event' },
    { label: 'Offers', icon: 'description' },
    { label: 'Analytics', icon: 'analytics' },
  ];
}
