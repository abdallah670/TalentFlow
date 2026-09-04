import { Component } from '@angular/core';
import { PortalSidebarComponent } from '@shared/components/portal-sidebar/portal-sidebar.component';

@Component({
  selector: 'app-interview-preparation',
  standalone: true,
  imports: [PortalSidebarComponent],
  templateUrl: './interview-preparation.component.html',
  styleUrl: './interview-preparation.component.scss',
})
export class InterviewPreparationComponent {}
