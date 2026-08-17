import { Component, Input, computed } from '@angular/core';

@Component({
  selector: 'app-step-indicator',
  standalone: true,
  templateUrl: './step-indicator.component.html',
  styleUrl: './step-indicator.component.scss',
})
export class StepIndicatorComponent {
  @Input({ required: true }) currentStep!: number;
  @Input({ required: true }) totalSteps!: number;
  
  steps = computed(() => Array.from({ length: this.totalSteps }, (_, i) => i + 1));
}
