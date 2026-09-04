import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './empolyersidebar.component.html',
  styleUrl: './empolyersidebar.component.scss',
})
export class SidebarComponent {
  public router = inject(Router);
  
  isCompleted(path: string): boolean {
     const routes = ['/register/company-setup', '/register/workspace', '/register/subscription', '/register/review'];
     const currentIndex = routes.indexOf(this.router.url.split('?')[0]);
     const itemIndex = routes.indexOf(path);
     return itemIndex < currentIndex;
  }
}
