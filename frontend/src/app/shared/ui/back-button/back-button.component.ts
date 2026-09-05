import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-back-button',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <button
      type="button"
      class="back-button"
      (click)="location.back()"
      aria-label="Go back to the previous page"
      title="Go back">
      <span class="material-symbols-outlined">arrow_back</span>
    </button>
  `,
  styles: [
    `
      .back-button {
        position: fixed;
        top: 24px;
        right: 24px;
        z-index: 1000;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 48px;
        height: 48px;
        border: none;
        border-radius: 50%;
        background: var(--primary, #24389c);
        color: #ffffff;
        cursor: pointer;
        box-shadow: 0 4px 16px rgba(36, 56, 156, 0.35);
        transition:
          transform 0.2s ease,
          box-shadow 0.2s ease,
          background-color 0.2s ease;
      }

      .back-button:hover {
        background: var(--primary-container, #3f51b5);
        box-shadow: 0 6px 20px rgba(36, 56, 156, 0.45);
        transform: translateY(-2px);
      }

      .back-button:active {
        transform: translateY(0);
      }

      .back-button .material-symbols-outlined {
        font-size: 24px;
        user-select: none;
      }
    `,
  ],
})
export class BackButtonComponent {
  protected readonly location = inject(Location);
}
