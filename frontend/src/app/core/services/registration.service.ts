import { Injectable, inject, signal } from '@angular/core';
import { Step, CandidateRegistrationRequest } from '../../data/models/registration.model';
import { AuthService } from './auth.service';
import { RegistrationResponse } from '../../data/models/auth.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CandidateRegistrationService {
  private readonly authService = inject(AuthService);

  readonly currentStep = signal(1);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly submitted = signal(false);

  readonly steps: Step[] = [
    { id: 1, label: 'Step 1', title: 'Personal Details' },
    { id: 2, label: 'Step 2', title: 'Professional Profile' },
    { id: 3, label: 'Step 3', title: 'Skills & Expertise' },
    { id: 4, label: 'Step 4', title: 'Review & Submit' }
  ];

  // Track validity of each step (1-based index)
  readonly stepValid = signal<Record<number, boolean>>({ 1: false, 2: true, 3: true, 4: true });

  readonly profile = signal<CandidateRegistrationRequest>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    currentJobTitle: '',
    currentCompany: '',
    totalYearsOfExperience: 0,
    linkedinUrl: '',
    portfolioUrl: '',
    skills: [],
    coverLetter: '',
    preferredLocation: '',
    remoteOnly: false,
    salaryExpectationMin: 0,
    salaryExpectationMax: 0,
    currency: '',
    availableFrom: '',
    workAuthorization: '',
  });

  setStep(step: number) {
    if (step >= 1 && step <= this.steps.length) {
      this.currentStep.set(step);
    }
  }

  nextStep() {
    this.setStep(this.currentStep() + 1);
  }

  prevStep() {
    this.setStep(this.currentStep() - 1);
  }

  updateProfile(data: Partial<CandidateRegistrationRequest>) {
    this.profile.update(p => ({ ...p, ...data }));
  }

  setStepValid(step: number, valid: boolean) {
    this.stepValid.update(s => ({ ...s, [step]: valid }));
  }

  isStepValid(step: number): boolean {
    return this.stepValid()[step] ?? false;
  }

  submit(): Observable<RegistrationResponse> {
    this.loading.set(true);
    this.error.set(null);
    this.submitted.set(false);

    const profile = this.profile();

    return this.authService.registerCandidate(profile);
  }

  reset() {
    this.currentStep.set(1);
    this.loading.set(false);
    this.error.set(null);
    this.submitted.set(false);
    this.stepValid.set({ 1: false, 2: true, 3: true, 4: true });
    this.profile.set({
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
      phoneNumber: '',
      currentJobTitle: '',
      currentCompany: '',
      totalYearsOfExperience: 0,
      linkedinUrl: '',
      portfolioUrl: '',
      skills: [],
      coverLetter: '',
      preferredLocation: '',
      remoteOnly: false,
      salaryExpectationMin: 0,
      salaryExpectationMax: 0,
      currency: '',
      availableFrom: '',
      workAuthorization: '',
    });
  }
}