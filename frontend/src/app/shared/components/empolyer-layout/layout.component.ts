import { Component } from '@angular/core';
import { SidebarComponent } from '../empolyer-sidebar/empolyersidebar.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [SidebarComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent {}
