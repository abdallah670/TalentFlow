import { Component } from '@angular/core';
import { PortalSidebarComponent } from '../../../components/shared/portal-sidebar/portal-sidebar.component';

@Component({
  selector: 'app-interview-prep-guide',
  standalone: true,
  imports: [PortalSidebarComponent],
  templateUrl: './interview-prep-guide.component.html',
  styleUrl: './interview-prep-guide.component.scss',
})
export class InterviewPrepGuideComponent {}
