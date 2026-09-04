import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface QuestionMapItem {
  number: number;
  status: 'answered' | 'current' | 'unanswered' | 'flagged';
}

@Component({
  selector: 'app-technical-assessment',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './technical-assessment.component.html',
  styleUrl: './technical-assessment.component.scss',
})
export class TechnicalAssessmentComponent {
  questionMap: QuestionMapItem[] = [
    { number: 1, status: 'answered' },
    { number: 2, status: 'answered' },
    { number: 3, status: 'answered' },
    { number: 4, status: 'answered' },
    { number: 5, status: 'answered' },
    { number: 6, status: 'answered' },
    { number: 7, status: 'current' },
    { number: 8, status: 'unanswered' },
    { number: 9, status: 'unanswered' },
    { number: 10, status: 'flagged' },
    { number: 11, status: 'unanswered' },
    { number: 12, status: 'unanswered' },
    { number: 13, status: 'unanswered' },
    { number: 14, status: 'unanswered' },
    { number: 15, status: 'unanswered' },
  ];

  options = [
    {
      value: 'a',
      title: 'API Gateway Pattern',
      description:
        'Use a central gateway to route requests and manage state across all services synchronously.',
    },
    {
      value: 'b',
      title: 'Circuit Breaker Pattern',
      description:
        'Prevent cascading failures by immediately failing calls to the PaymentService if it becomes unresponsive.',
    },
    {
      value: 'c',
      title: 'Saga Pattern',
      description:
        'Implement a sequence of local transactions where each service updates its data and publishes an event or message to trigger the next step. If a step fails, compensating transactions are executed to undo preceding steps.',
    },
    {
      value: 'd',
      title: 'Event Sourcing',
      description:
        'Store the state of all services as a sequence of state-changing events in an append-only log.',
    },
  ];
}
