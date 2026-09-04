import { Component } from '@angular/core';
import { PortalSidebarComponent } from '@shared/components/portal-sidebar/portal-sidebar.component';

@Component({
  selector: 'app-assessment-results',
  standalone: true,
  imports: [PortalSidebarComponent],
  templateUrl: './assessment-results.component.html',
  styleUrl: './assessment-results.component.scss',
})
export class AssessmentResultsComponent {
  categories = [
    { name: 'System Design', score: 90 },
    { name: 'Algorithms', score: 80 },
    { name: 'Database Optimization', score: 100 },
  ];

  questions = [
    {
      number: 1,
      title: 'Explain the CAP theorem in the context of distributed systems.',
      tag: 'System Design',
      status: 'correct' as const,
      candidateAnswer:
        'The CAP theorem states that a distributed data store can only guarantee two out of three characteristics simultaneously: Consistency, Availability, and Partition tolerance. In presence of a network partition, one has to choose between consistency and availability.',
      correctAnswer: null as string | null,
      scoreLabel: 'Score: 10/10',
      scoreClass: 'ar-score--correct',
    },
    {
      number: 2,
      title: 'What is the time complexity of searching in a perfectly balanced binary search tree?',
      tag: 'Algorithms',
      status: 'incorrect' as const,
      candidateAnswer: 'O(n log n)',
      correctAnswer: 'O(log n)',
      scoreLabel: 'Score: 0/5',
      scoreClass: 'ar-score--incorrect',
    },
  ];
}
