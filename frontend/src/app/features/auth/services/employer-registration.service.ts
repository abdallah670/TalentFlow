import { Injectable, inject, signal } from '@angular/core';
import { AuthService } from './auth.service';

import { Observable } from 'rxjs';
import { EmployerRegistrationRequest } from '../models/registration.model';

@Injectable({
  providedIn: 'root'
})
export class EmployerRegistrationService {
  private readonly authService = inject(AuthService);

  readonly currentStep = signal(1);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly submitted = signal(false);

  readonly steps = [
    { id: 1, label: 'Step 1', title: 'Account' },
    { id: 2, label: 'Step 2', title: 'Role' },
    { id: 3, label: 'Step 3', title: 'Company Details' },
    { id: 4, label: 'Step 4', title: 'Subscription Plan' },
  ];

  readonly stepValid = signal<Record<number, boolean>>({
    1: false,
    2: false,
    3: false,
    4: false,
  });

  readonly profile = signal<EmployerRegistrationRequest>({
    firstName: '',
    lastName: '',
    email: '',
    companyName: '',
    password: '',
    confirmPassword: '',
    companySize: '',
    industry: '',
    websiteUrl: '',
    linkedinUrl: '',
    officeLocation: '',
    selectedPlan: '',
    roleType: '',
    otherRoleDetail: '',
    workspaceName: '',
    workspaceUrl: '',
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

  updateProfile(data: Partial<EmployerRegistrationRequest>) {
    this.profile.update(p => ({ ...p, ...data }));
  }

  setStepValid(step: number, valid: boolean) {
    this.stepValid.update(s => ({ ...s, [step]: valid }));
  }

  isStepValid(step: number): boolean {
    return this.stepValid()[step] ?? false;
  }

  submit(): Observable<any> {
    this.loading.set(true);
    this.error.set(null);
    this.submitted.set(false);

    const profile = this.profile();
    return this.authService.registerEmployer(profile);
  }

  reset() {
    this.currentStep.set(1);
    this.loading.set(false);
    this.error.set(null);
    this.submitted.set(false);
    this.stepValid.set({ 1: false, 2: false, 3: false, 4: false });
    this.profile.set({
      firstName: '',
      lastName: '',
      email: '',
      companyName: '',
      password: '',
      confirmPassword: '',
      companySize: '',
      industry: '',
      websiteUrl: '',
      linkedinUrl: '',
      officeLocation: '',
      selectedPlan: '',
      roleType: '',
      otherRoleDetail: '',
      workspaceName: '',
      workspaceUrl: '',
    });
  }
}