import { Component } from '@angular/core';
import { PortalSidebarComponent } from '@shared/components/portal-sidebar/portal-sidebar.component';

@Component({
  selector: 'app-comparison-matrix',
  standalone: true,
  imports: [PortalSidebarComponent],
  templateUrl: './comparison-matrix.component.html',
  styleUrl: './comparison-matrix.component.scss',
})
export class ComparisonMatrixComponent {}
