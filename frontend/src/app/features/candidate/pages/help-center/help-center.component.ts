import { Component } from '@angular/core';
import { PortalSidebarComponent } from '@shared/components/portal-sidebar/portal-sidebar.component';

@Component({
  selector: 'app-help-center',
  standalone: true,
  imports: [PortalSidebarComponent],
  templateUrl: './help-center.component.html',
  styleUrl: './help-center.component.scss',
})
export class HelpCenterComponent {}
