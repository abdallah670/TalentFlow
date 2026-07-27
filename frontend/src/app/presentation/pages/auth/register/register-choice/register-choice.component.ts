import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-choice',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './register-choice.component.html',
  styleUrl: './register-choice.component.scss',
})
export class RegisterChoiceComponent {
  private readonly router = inject(Router);

  navigateToCandidate(): void {
    this.router.navigate(['/register/candidate']);
  }

  navigateToEmployer(): void {
    this.router.navigate(['/register/employer']);
  }
}
