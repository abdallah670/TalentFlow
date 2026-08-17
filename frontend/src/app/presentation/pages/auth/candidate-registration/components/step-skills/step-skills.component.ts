import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateRegistrationService } from '../../../../../../core/services/registration.service';

@Component({
  selector: 'app-step-skills',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './step-skills.component.html',
  styleUrl: './step-skills.component.scss'
})
export class StepSkillsComponent {
  readonly registrationService = inject(CandidateRegistrationService);
  skills = this.registrationService.profile;

  removeSkill(skillToRemove: string) {
    const currentSkills = this.skills().skills;
    const updated = (currentSkills || []).filter((s: string) => s !== skillToRemove);
    this.registrationService.updateProfile({ skills: updated });
  }

  addSkill(event: Event) {
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();
    if (value && event instanceof KeyboardEvent && event.key === 'Enter') {
      event.preventDefault();
      const currentSkills = this.skills().skills || [];
      if (!currentSkills.includes(value) && currentSkills.length < 10) {
        this.registrationService.updateProfile({
          skills: [...currentSkills, value]
        });
      }
      input.value = '';
    }
  }

  onCoverLetterChange(event: Event) {
    const textarea = event.target as HTMLTextAreaElement;
    this.registrationService.updateProfile({ coverLetter: textarea.value });
  }

  get coverLetter(): string {
    return this.skills().coverLetter || '';
  }

  get maxCoverLetterLength(): number {
    return 2000;
  }

  get remainingChars(): number {
    return this.maxCoverLetterLength - (this.coverLetter?.length || 0);
  }
}
